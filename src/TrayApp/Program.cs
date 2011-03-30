/*
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010, 2011 Steven Robertson <steve@playnode.com>
 *
 * Windar - Playdar for Windows
 *
 * Windar is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License (LGPL) as published
 * by the Free Software Foundation; either version 2.1 of the License, or (at
 * your option) any later version.
 *
 * Windar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License version 2.1 for more details
 * (a copy is included in the LICENSE file that accompanied this code).
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using log4net;
using Microsoft.Win32;
using Windar.Common;
using Windar.PlaydarDaemon;
using Windar.TrayApp.Configuration;

namespace Windar.TrayApp
{
    class Program
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        internal delegate void ScanCompletedCallback();

        static Program _instance;

        internal static Program Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        internal string PlaydarDaemon
        {
            get
            {
                StringBuilder result = new StringBuilder();
                //TODO: Get the configured URL from config.
                result.Append("http://127.0.0.1:");
                int port = 60211;
                if (Config != null) port = Config.MainConfig.WebPort;
                result.Append(port).Append('/');
                return result.ToString();
            }
        }

        WindarPaths _paths;
        MainForm _mainForm;
        DaemonController _daemon;
        Tray _tray;
        PluginHost _pluginHost;
        Queue<string> _scanQueue;
        bool _indexing;
        WaitingDialog _waitingDialog;

        internal WindarPaths Paths
        {
            get { return _paths; }
            set { _paths = value; }
        }

        internal MainForm MainForm
        {
            get { return _mainForm; }
            set { _mainForm = value; }
        }

        internal DaemonController Daemon
        {
            get { return _daemon; }
            set { _daemon = value; }
        }

        internal Tray Tray
        {
            get { return _tray; }
            set { _tray = value; }
        }

        internal PluginHost PluginHost
        {
            get { return _pluginHost; }
            set { _pluginHost = value; }
        }

        internal Queue<string> ScanQueue
        {
            get { return _scanQueue; }
            set { _scanQueue = value; }
        }

        internal bool Indexing
        {
            get { return _indexing; }
            set { _indexing = value; }
        }

        internal WaitingDialog WaitingDialog
        {
            get { return _waitingDialog; }
            set { _waitingDialog = value; }
        }

        internal class ConfigGroup
        {
            MainConfigFile _mainConfig;
            TcpConfigFile _peersConfig;

            public MainConfigFile MainConfig
            {
                get { return _mainConfig; }
                set { _mainConfig = value; }
            }

            public TcpConfigFile PeersConfig
            {
                get { return _peersConfig; }
                set { _peersConfig = value; }
            }
        }

        ConfigGroup _config;

        // NOTE: Config set to null on config load exception.
        internal ConfigGroup Config
        {
            get { return _config; }
            set { _config = value; }
        }

        #region Win32 API

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion

        [STAThread]
        static void Main()
        {
            // Ensure only one instance of application runs.
            bool notRunning;
            using (new Mutex(true, "Windar", out notRunning))
            {
                if (!notRunning)
                {
                    // If already running, bring it to the front.
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id == current.Id) continue;
                        SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
                else
                {
                    if (Log.IsInfoEnabled) Log.Info("Started.");

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    SetupSessionEndingHandler();
                    SetupShutdownFileWatcher();

                    // Application exception handling.
                    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                    AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;
                    Application.ThreadException += HandleThreadException;

                    Instance = new Program();

#if DEBUG
                    Instance.Run();
#else
                    try
                    {
                        Instance.Run();
                    }
                    catch (Exception ex)
                    {
                        if (Log.IsErrorEnabled) Log.Error("Exception in Main", ex);
                    }
#endif

                    if (Log.IsInfoEnabled) Log.Info("Finished.");
                }
            }
        }

        Program()
        {
            Instance = this;
            
            Config = new ConfigGroup();
            Paths = new WindarPaths(Application.StartupPath);
            Daemon = new DaemonController(Paths);
            ScanQueue = new Queue<string>();
            MainForm = new MainForm();
            Tray = new Tray();
            PluginHost = new PluginHost(Paths);
            WaitingDialog = new WaitingDialog();

            // Register command event handlers.
            Daemon.ScanCompleted += ScanCompleted;
            Daemon.PlaydarStopped += PlaydarStopped;
            Daemon.PlaydarStarted += PlaydarStarted;
            Daemon.PlaydarStartFailed += PlaydarStartFailed;
        }

        void Run()
        {
            // Attempt to load the config files.
            if (!LoadConfiguration()) return;

            CheckConfig();
            PluginHost.Load();
            Daemon.Start();

            if (Properties.Settings.Default.MainFormVisible)
            {
                MainForm.EnsureVisible();
            }
            else
            {
                // Need to call the EnsureVisible method to ensure
                // that the scan files from tray menu works nicely.
                MainForm.Opacity = 0;
                MainForm.EnsureVisible();
                MainForm.Hide();
                MainForm.Opacity = 1;
            }
            Application.Run(Tray);
        }

        internal static void Shutdown()
        {
            if (Log.IsInfoEnabled) Log.Info("Shutting down.");

            if (Instance.PluginHost != null)
            {
                try
                {
                    Instance.PluginHost.Shutdown();
                }
                catch (Exception ex)
                {
                    if (Log.IsErrorEnabled) Log.Error("PluginHost shutdown exception.", ex);
                }
            }

            if (Instance.Daemon != null)
            {
                try
                {
                    Instance.Daemon.Stop();
                }
                catch (Exception ex)
                {
                    if (Log.IsErrorEnabled) Log.Error("Daemon shutdown exception.", ex);
                }
            }

            if (Instance.Tray != null)
            {
                try
                {
                    if (Instance.Tray.NotifyIcon != null) 
                        Instance.Tray.NotifyIcon.Visible = false;
                    Instance.Tray.Close();
                }
                catch (Exception ex)
                {
                    if (Log.IsErrorEnabled) Log.Error("Tray shutdown exception.", ex);
                }
            }
            
            if (Instance.WaitingDialog != null)
                Instance.WaitingDialog.Stop();
            
            if (Instance.MainForm == null) return;

            try
            {
                Instance.MainForm.Exit();
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled) Log.Error("MainForm shutdown exception.", ex);
            }
        }

        #region Uninstaller shutdown file-watcher.

        static void SetupShutdownFileWatcher()
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Application.StartupPath;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "SHUTDOWN";

            // Add event handlers.
            watcher.Changed += ShutdownFile_OnChanged;
            watcher.Created += ShutdownFile_OnChanged;
            watcher.Deleted += ShutdownFile_OnChanged;
            watcher.Renamed += ShutdownFile_OnChanged;

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        static void ShutdownFile_OnChanged(object source, FileSystemEventArgs e)
        {
            if (Log.IsInfoEnabled) Log.Info("Shutdown initiated by the file-system event.");
            Shutdown();
        }

        #endregion

        #region Session ending

        static void SetupSessionEndingHandler()
        {
            if (Log.IsDebugEnabled) Log.Debug("Setting up handler for SessionEnding event.");
            SystemEvents.SessionEnding += SystemEvents_OnSessionEnding;
        }

        static void SystemEvents_OnSessionEnding(object sender, EventArgs e)
        {
            if (Log.IsInfoEnabled) Log.Info("Session ending! Initiating shutdown.");
            Shutdown();
        }

        #endregion

        #region Application exception handling.

        public static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleUnhandledException("Windar Program Exception", (Exception) e.ExceptionObject);
        }

        public static void HandleThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleUnhandledException("Windar Thread Exception", e.Exception);
        }

        static void HandleUnhandledException(string msg, Exception ex)
        {
            string errMsg = null;
            if (Log.IsErrorEnabled) Log.Error(msg, ex);

            if (ex != null && ex.Message != null) errMsg = ex.Message;
            StringBuilder msgBuild = new StringBuilder();
            if (errMsg != null)
            {
                msgBuild.Append(errMsg);
                if (!(errMsg[errMsg.Length - 1] == '.' || errMsg[errMsg.Length - 1] == '!')) 
                    msgBuild.Append('.');
                msgBuild.Append(' ');
            }
            
            msgBuild.Append("Quit program?");
            
            DialogResult result = MessageBox.Show(msgBuild.ToString(),
                                         msg, 
                                         MessageBoxButtons.YesNo, 
                                         MessageBoxIcon.Exclamation,
                                         MessageBoxDefaultButton.Button2);
            
            if (result != DialogResult.Yes) return;
            
            //if (MessageBox.Show("Are you sure?", "Confirm quit program", 
            //    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                
            Shutdown();
        }

        #endregion

        #region Common dialogs.

        internal static void ShowInfoDialog(string msg)
        {
            MessageBox.Show(msg, "Windar Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        internal static bool ShowYesNoDialog(string msg)
        {
            DialogResult result = MessageBox.Show(msg, "Windar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        internal static DialogResult ShowYesNoCancelDialog(string msg)
        {
            return MessageBox.Show(msg, "Windar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        internal static void ShowErrorDialog(Exception ex)
        {
            if (string.IsNullOrEmpty(ex.Message)) ShowErrorDialog("Exception: " + ex.GetType().Name);
            else ShowErrorDialog("Exception: " + ex.Message);
        }

        internal static void ShowErrorDialog(string msg)
        {
            MessageBox.Show(msg, "Windar Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        internal static void ShowApplyChangesDialog()
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("To apply changes you will also need to restart Playdar.");
            msg.Append(Environment.NewLine).Append("Do you want to restart Playdar now?");
            if (ShowYesNoDialog(msg.ToString())) Instance.Daemon.Restart();
        }

        #endregion

        #region Tray notifications.

        internal void ShowTrayInfo(string msg)
        {
            ShowTrayInfo(msg, ToolTipIcon.Info);
        }

        internal void ShowTrayInfo(string msg, ToolTipIcon icon)
        {
            if (!Properties.Settings.Default.ShowBalloons) return;
            Tray.NotifyIcon.Visible = true;
            Tray.NotifyIcon.ShowBalloonTip(3, "Windar", msg, icon);
        }

        #endregion

        #region Assembly attributes.

        internal static string AssemblyTitle
        {
            get
            {
                // Get all Title attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];

                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "") return titleAttribute.Title;
                }

                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        internal static string AssemblyVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        internal static string AssemblyDescription
        {
            get
            {
                // Get all Description attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

                // If there aren't any Description attributes, return an empty string
                // If there is a Description attribute, return its value
                return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        internal static string AssemblyProduct
        {
            get
            {
                // Get all Product attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);

                // If there aren't any Product attributes, return an empty string
                // If there is a Product attribute, return its value
                return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        internal static string AssemblyCopyright
        {
            get
            {
                // Get all Copyright attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

                // If there aren't any Copyright attributes, return an empty string
                // If there is a Copyright attribute, return its value
                return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        internal static string AssemblyCompany
        {
            get
            {
                // Get all Company attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

                // If there aren't any Company attributes, return an empty string
                // If there is a Company attribute, return its value
                return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        internal static string AssemblyTrademark
        {
            get
            {
                // Get all Trademark attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTrademarkAttribute), false);

                // If there aren't any Trademark attributes, return an empty string
                // If there is a Trademark attribute, return its value
                return attributes.Length == 0 ? "" : ((AssemblyTrademarkAttribute)attributes[0]).Trademark;
            }
        }

        #endregion

        #region Daemon lifecycle.

        internal void StartDaemon()
        {
            Instance.MainForm.startDaemonButton.Enabled = false;
            Application.DoEvents();
            Instance.Daemon.Start();
        }

        internal void StopDaemon()
        {
            Instance.MainForm.stopDaemonButton.Enabled = false;
            Instance.MainForm.restartDaemonButton.Enabled = false;
            Instance.MainForm.ShowDaemonPage();
            Instance.MainForm.homeButton.Enabled = false;
            Instance.MainForm.backButton.Enabled = false;
            Instance.MainForm.refreshButton.Enabled = false;
            Application.DoEvents();
            Instance.Daemon.Stop();
        }

        internal void RestartDaemon()
        {
            if (!Instance.Daemon.Started) return;
            Instance.MainForm.ShowDaemonPage("Restarting", false);
            Instance.MainForm.startDaemonButton.Enabled = false;
            Instance.MainForm.stopDaemonButton.Enabled = false;
            Instance.MainForm.restartDaemonButton.Enabled = false;
            Instance.MainForm.homeButton.Enabled = false;
            Instance.MainForm.backButton.Enabled = false;
            Instance.MainForm.refreshButton.Enabled = false;
            Application.DoEvents();
            Instance.Daemon.Restart();
            if (!Instance.Daemon.Started) return;
            Instance.MainForm.PlaydarStateChanged(true); // Required due to timing issue.
            Instance.MainForm.playdarBrowser.Navigate(PlaydarDaemon);
        }

        #endregion

        internal void AddScanPath(string path)
        {
            ScanQueue.Enqueue(path);
            if (Indexing) return;
            Indexing = true;
            Daemon.Scan(ScanQueue.Dequeue());
        }

        #region Daemon event handlers.

        void PlaydarStopped(object sender, EventArgs e)
        {
            ShowTrayInfo("Playdar stopped.");
        }

        void PlaydarStarted(object sender, EventArgs e)
        {
            ShowTrayInfo("Playdar started.");
        }

        void PlaydarStartFailed(object sender, EventArgs e)
        {
            ShowTrayInfo("Playdar failed to start!");
        }

        void ScanCompleted(object sender, EventArgs e)
        {
            if (ScanQueue.Count > 0) Daemon.Scan(ScanQueue.Dequeue());
            else
            {
                Indexing = false;
                ShowTrayInfo("Scan completed.");
                ScanCompletedCallback d = new ScanCompletedCallback(MainForm.ScanCompleted);
                if (MainForm != null) MainForm.Invoke(d);
            }
        }

        #endregion

        #region Configuration files.

        internal void SaveConfiguration()
        {
            Config.MainConfig.Save();

            // Always disable default sharing as it conflicts with the peer
            // configuration provided by this UI for now.
            Config.PeersConfig.DefaultShare = false;
            Config.PeersConfig.Save();
        }

        /// <summary>
        /// Load configuration from files.
        /// </summary>
        /// <returns>True if configuration loaded ok, false otherwise.</returns>
        internal bool LoadConfiguration()
        {
            bool result = false;
            try
            {
                // Determine file paths.
                string path = Paths.WindarAppData;
                FileInfo main = new FileInfo(path + @"\etc\playdar.conf");
                FileInfo peer = new FileInfo(path + @"\etc\playdartcp.conf");

                // Check the files exist.
                if (!main.Exists) throw new WindarException("Main config file not found!");
                if (!peer.Exists) throw new WindarException("Playdar TCP config file not found!");

                // Load main config.
                Config.MainConfig = new MainConfigFile();
                Config.MainConfig.Load(main);

                // Load peers config.
                Config.PeersConfig = new TcpConfigFile();
                Config.PeersConfig.Load(peer);

                result = true;
            }
            catch (WindarException ex)
            {
                Config = null;
                if (Log.IsErrorEnabled) Log.Error(ex.Message, ex);
                ShowErrorDialog(ex.Message);
                Shutdown();
            }
            catch (Exception ex)
            {
                Config = null;
                StringBuilder msg = new StringBuilder();
                msg.Append("Exception when reading configuration files.");
                if (Log.IsErrorEnabled) Log.Error(msg, ex);
                //msg.Append(Environment.NewLine).Append(ex.Message);
                ShowErrorDialog(msg.ToString());
                Shutdown();
            }

            return result;
        }

        void CheckConfig()
        {
            string path = Paths.WindarAppData;
            path = path.Replace('\\', '/');
            bool update = (Config.MainConfig.LibraryDbDir != path)
                || (Config.MainConfig.AuthDbDir != path);

            if (!update) return;
            Config.MainConfig.LibraryDbDir = path;
            Config.MainConfig.AuthDbDir = path;
            SaveConfiguration();
        }

        #endregion

        internal static string WGet(string url, int timeout)
        {
            string result = null;

            if (Log.IsDebugEnabled) Log.Debug("WGet " + url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = timeout;
            request.UserAgent = "Windar";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Encoding enc = Encoding.GetEncoding(1252);
                    StreamReader stream = new StreamReader(response.GetResponseStream(), enc);
                    result = stream.ReadToEnd();
                    response.Close();
                    stream.Close();
                    if (Log.IsDebugEnabled) Log.Debug("WGet result:\n" + result);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    if (Log.IsErrorEnabled) Log.Error("Exception", ex);
                }
                else
                {
                    switch (((HttpWebResponse) ex.Response).StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            if (Log.IsErrorEnabled) Log.Error("404 Not Found");
                            break; // Ignore.
                        default:
                            if (Log.IsErrorEnabled) Log.Error("Exception", ex);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled) Log.Error("Exception", ex);
            }
            return result;
        }

        internal bool FindMPlayer()
        {
            string path = new StringBuilder(Paths.WindarProgramFiles).Append(@"\mplayer\mplayer.exe").ToString();
            if (Log.IsDebugEnabled) Log.Debug("Looking for MPlayer. Path = \"" + path + "\"");
            return File.Exists(path);
        }
    }
}
