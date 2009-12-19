/*
 * Windar: Playdar for Windows
 * Copyright (C) 2009 Steven Robertson <steve@playnode.org>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using log4net;
using Windar.Common;
using Windar.PlaydarDaemon;
using Windar.TrayApp.Configuration;

namespace Windar.TrayApp
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        internal static Program Instance { get; private set; }

        internal string PlaydarDaemon
        {
            get
            {
                var result = new StringBuilder();
                result.Append("http://localhost:");
                var port = 60211;
                if (Config != null) port = Config.Main.WebPort;
                result.Append(port).Append('/');
                return result.ToString();
            }
        }

        internal WindarPaths Paths { get; private set; }
        internal MainForm MainForm { get; private set; }
        internal DaemonController Daemon { get; private set; }
        internal Tray Tray { get; private set; }
        internal PluginHost PluginHost { get; private set; }
        internal Queue<string> ScanQueue { get; private set; }

        internal class ConfigGroup
        {
            public MainConfigFile Main { get; set; }
            public TcpConfigFile Peers { get; set; }
        }

        // NOTE: Config set to null on config load exception.
        internal ConfigGroup Config { get; private set; }

        internal delegate void EnableReIndexButtonCallback();

        private bool _scanning;

        #region Win32 API

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion

        #region Init

        [STAThread]
        static void Main()
        {
            // Application exception handling.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;
            Application.ThreadException += HandleThreadException;

            // Ensure only one instance of application runs.
            bool notCurrentlyShown;
            using (new Mutex(true, "Windar", out notCurrentlyShown))
            {
                if (notCurrentlyShown)
                {
                    if (Log.IsInfoEnabled) Log.Info("Starting.");
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Instance = new Program();
                    _controlHandler += ControlHandler;
                    SetConsoleCtrlHandler(_controlHandler, true);
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
                    SetConsoleCtrlHandler(_controlHandler, false);
                    if (Log.IsInfoEnabled) Log.Info("Finished.");
                }
                else
                {
                    var current = Process.GetCurrentProcess();
                    foreach (var process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id == current.Id) continue;
                        SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
            }
        }

        private Program()
        {
            Instance = this;
            SetupShutdownFileWatcher();
            Config = new ConfigGroup();
            Paths = new WindarPaths(Application.StartupPath);
            Daemon = new DaemonController(Paths);
            ScanQueue = new Queue<string>();
            MainForm = new MainForm();
            Tray = new Tray();
            PluginHost = new PluginHost();

            // Register command event handlers.
            Daemon.ScanCompleted += ScanCompleted;
            Daemon.PlaydarStopped += PlaydarStopped;
            Daemon.PlaydarStarted += PlaydarStarted;
            Daemon.PlaydarStartFailed += PlaydarStartFailed;
        }

        private void Run()
        {
            // Attempt to load the config files.
            if (!LoadConfiguration()) return;

            CheckConfig();
            PluginHost.Load();
            Daemon.Start();

            if (Properties.Settings.Default.MainFormVisible) MainForm.EnsureVisible();
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

        #endregion

        #region Program exit.

        internal static void Shutdown()
        {
            Shutdown(false);
        }

        internal static void Shutdown(bool cancelSave)
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
            if (Instance.MainForm != null)
            {
                try
                {
                    Instance.MainForm.Exit();
                }
                catch (Exception ex)
                {
                    if (Log.IsErrorEnabled) Log.Error("MainForm shutdown exception.", ex);
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
            Application.Exit();
        }

        #region So far unsuccessful attempt to get file-system shutdown/log-off events.

        private enum ControlEventType
        {
            CtrlCEvent = 0,
            CtrlBreakEvent = 1,
            CtrlCloseEvent = 2,
            CtrlLogoffEvent = 5,
            CtrlShutdownEvent = 6,
        }

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(HandlerDelegate handlerRoutine, bool add);

        private delegate bool HandlerDelegate(ControlEventType dwControlType);

        private static HandlerDelegate _controlHandler;

        private static bool ControlHandler(ControlEventType controlEvent)
        {
            // TODO: Either get this method to work, replace or delete it.
            if (Log.IsDebugEnabled) Log.Debug("Control event (" + controlEvent + ')');
            var result = false;
            switch (controlEvent)
            {
                case ControlEventType.CtrlCEvent:
                case ControlEventType.CtrlBreakEvent:
                case ControlEventType.CtrlCloseEvent:
                case ControlEventType.CtrlLogoffEvent:
                case ControlEventType.CtrlShutdownEvent:
                    // Return true to show that the event was handled.
                    result = true;
                    Shutdown(false);
                    break;
            }
            return result;
        }

        #endregion

        #region Uninstaller shutdown file-watcher.

        private static void SetupShutdownFileWatcher()
        {
            // Create a new FileSystemWatcher and set its properties.
            var watcher = new FileSystemWatcher
            {
                Path = Application.StartupPath,
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                               | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "SHUTDOWN"
            };

            // Add event handlers.
            watcher.Changed += ShutdownFile_OnChanged;
            watcher.Created += ShutdownFile_OnChanged;
            watcher.Deleted += ShutdownFile_OnChanged;
            watcher.Renamed += ShutdownFile_OnChanged;

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        private static void ShutdownFile_OnChanged(object source, FileSystemEventArgs e)
        {
            if (Log.IsInfoEnabled) Log.Info("Shutdown initiated by the file-system event.");
            Shutdown();
        }

        #endregion

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

        private static void HandleUnhandledException(string msg, Exception ex)
        {
            string errMsg = null;
            if (Log.IsErrorEnabled) Log.Error(msg, ex);
            if (ex != null && ex.Message != null) errMsg = ex.Message;
            var msgBuild = new StringBuilder();
            if (errMsg != null)
            {
                msgBuild.Append(errMsg);
                if (!(errMsg[errMsg.Length - 1] == '.' || errMsg[errMsg.Length - 1] == '!')) 
                    msgBuild.Append('.');
                msgBuild.Append(' ');
            }
            msgBuild.Append("Quit program?");
            var result = MessageBox.Show(msgBuild.ToString(),
                                         msg, 
                                         MessageBoxButtons.YesNo, 
                                         MessageBoxIcon.Exclamation,
                                         MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes) return;
            if (MessageBox.Show("Are you sure?", "Confirm quit program", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
            var result = MessageBox.Show(msg, "Windar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

        #endregion

        #region Tray notifications.

        internal void ShowTrayInfo(string msg)
        {
            ShowTrayInfo(msg, false);
        }

        internal void ShowTrayInfo(string msg, bool persist)
        {
            ShowTrayInfo(msg, ToolTipIcon.Info, persist);
        }

        internal void ShowTrayInfo(string msg, ToolTipIcon icon)
        {
            ShowTrayInfo(msg, icon, false);
        }

        internal void ShowTrayInfo(string msg, ToolTipIcon icon, bool persist)
        {
            if (!Properties.Settings.Default.ShowBalloons) return;
            Tray.NotifyIcon.Visible = true;
            var timeout = persist ? 60*60 : 3;
            Tray.NotifyIcon.ShowBalloonTip(timeout, "Windar", msg, icon);
        }

        #endregion

        #region Assembly attributes.

        internal static string AssemblyTitle
        {
            get
            {
                // Get all Title attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    var titleAttribute = (AssemblyTitleAttribute)attributes[0];

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
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

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
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);

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
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

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
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

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
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTrademarkAttribute), false);

                // If there aren't any Trademark attributes, return an empty string
                // If there is a Trademark attribute, return its value
                return attributes.Length == 0 ? "" : ((AssemblyTrademarkAttribute)attributes[0]).Trademark;
            }
        }

        #endregion

        #region Daemon lifecycle.

        internal void StartDaemon()
        {
            Instance.Daemon.Start();
            Instance.MainForm.StartDaemonButton.Enabled = false;
            Instance.MainForm.StopDaemonButton.Enabled = true;
            Instance.MainForm.RestartDaemonButton.Enabled = true;
            Instance.MainForm.LoadPlaydarHomepage();
            Instance.MainForm.RefreshButton.Enabled = true;
        }

        internal void StopDaemon()
        {
            Instance.Daemon.Stop();
            Instance.MainForm.StartDaemonButton.Enabled = true;
            Instance.MainForm.StopDaemonButton.Enabled = false;
            Instance.MainForm.RestartDaemonButton.Enabled = false;
            Instance.MainForm.ShowDaemonPage();
            Instance.MainForm.HomeButton.Enabled = false;
            Instance.MainForm.BackButton.Enabled = false;
            Instance.MainForm.RefreshButton.Enabled = false;
        }

        internal void RestartDaemon()
        {
            if (!Instance.Daemon.Started) return;
            Instance.MainForm.ShowDaemonPage("Restarting", false);
            Instance.MainForm.StartDaemonButton.Enabled = false;
            Instance.MainForm.StopDaemonButton.Enabled = false;
            Instance.MainForm.RestartDaemonButton.Enabled = false;
            Instance.MainForm.HomeButton.Enabled = false;
            Instance.MainForm.BackButton.Enabled = false;
            Instance.MainForm.RefreshButton.Enabled = false;
            Instance.Daemon.Restart();
            if (!Instance.Daemon.Started) return;
            Instance.MainForm.PlaydarBrowser.Navigate(PlaydarDaemon);
            Instance.MainForm.RefreshButton.Enabled = true;
            Instance.MainForm.StartDaemonButton.Enabled = false;
            Instance.MainForm.StopDaemonButton.Enabled = true;
            Instance.MainForm.RestartDaemonButton.Enabled = true;
        }

        #endregion

        internal void AddScanPath(string path)
        {
            ScanQueue.Enqueue(path);
            if (_scanning) return;
            _scanning = true;
            Daemon.Scan(ScanQueue.Dequeue());
        }

        #region Daemon event handlers.

        private void PlaydarStopped(object sender, EventArgs e)
        {
            ShowTrayInfo("Playdar stopped.");
        }

        private void PlaydarStarted(object sender, EventArgs e)
        {
            ShowTrayInfo("Playdar started.");
        }

        private void PlaydarStartFailed(object sender, EventArgs e)
        {
            ShowTrayInfo("Playdar failed to start!", true);
        }

        private void ScanCompleted(object sender, EventArgs e)
        {
            if (ScanQueue.Count > 0) Daemon.Scan(ScanQueue.Dequeue());
            else
            {
                ShowTrayInfo("Scan completed.", true);
                _scanning = false;

                if (MainForm.ReIndexButton.InvokeRequired)
                {
                    // It's on a different thread, so use Invoke.
                    var d = new EnableReIndexButtonCallback(EnableReIndexButton);
                    MainForm.ReIndexButton.Invoke(d, new object[] { });
                }
            }
        }

        private void EnableReIndexButton()
        {
            MainForm.ReIndexButton.Enabled = true;
        }

        #endregion

        #region Configuration files.

        internal void SaveConfiguration()
        {
            Config.Main.Save();

            // Always disable default sharing as it conflicts with the peer
            // configuration provided by this UI for now.
            Config.Peers.DefaultShare = false;
            Config.Peers.Save();
        }

        /// <summary>
        /// Load configuration from files.
        /// </summary>
        /// <returns>True if configuration loaded ok, false otherwise.</returns>
        internal bool LoadConfiguration()
        {
            var result = false;
            try
            {
                // Determine file paths.
                var path = Paths.PlaydarDataPath;
                var main = new FileInfo(path + @"\etc\playdar.conf");
                var peer = new FileInfo(path + @"\etc\playdartcp.conf");

                // Check the files exist.
                if (!main.Exists) throw new WindarException("Main config file not found!");
                if (!peer.Exists) throw new WindarException("Playdar TCP config file not found!");

                // Load main config.
                Config.Main = new MainConfigFile();
                Config.Main.Load(main);

                // Load peers config.
                Config.Peers = new TcpConfigFile();
                Config.Peers.Load(peer);

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
                var msg = new StringBuilder();
                msg.Append("Exception when reading configuration files.");
                if (Log.IsErrorEnabled) Log.Error(msg, ex);
                //msg.Append(Environment.NewLine).Append(ex.Message);
                ShowErrorDialog(msg.ToString());
                Shutdown();
            }

            return result;
        }

        private void CheckConfig()
        {
            var path = Paths.PlaydarDataPath;
            path = path.Replace('\\', '/');
            var update = (Config.Main.LibraryDbDir != path)
                || (Config.Main.AuthDbDir != path);

            if (!update) return;
            Config.Main.LibraryDbDir = path;
            Config.Main.AuthDbDir = path;
            SaveConfiguration();
        }

        #endregion
    }
}
