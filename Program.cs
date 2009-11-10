/*
 * Windar: Playdar for Windows
 * Copyright (C) 2009 Steven Robertson <steve.r@k-os.net>
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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using log4net;
using Windar.Commands;

namespace Windar
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        internal static Program Instance { get; private set; }

        internal DaemonBrowser PlaydarBrowser { get; private set; }

        private TrayApp _trayApp;

        #region Main

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew;
            using (new Mutex(true, "Windar", out createdNew))
            {
                if (createdNew)
                {
                    if (Log.IsInfoEnabled) Log.Info("Started.");
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
#if DEBUG
                    Instance = new Program();
                    Instance.Run();
#else
                    try
                    {
                        Instance = new Program();
                        Instance.Run();
                    }
                    catch (Exception ex)
                    {
                        if (Log.IsErrorEnabled) Log.Error("Exception in Main", ex);
                    }
#endif
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

        /*
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Log.IsInfoEnabled) Log.Info("Started.");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if DEBUG
            Instance = new Program();
            Instance.Run();
#else
            try
            {
                Instance = new Program();
                Instance.Run();
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled) Log.Error("Exception in Main", ex);
            }
#endif
            if (Log.IsInfoEnabled) Log.Info("Finished.");
        }
        */

        #endregion

        internal Program()
        {
            SetupShutdownFileWatcher();
            PlaydarBrowser = new DaemonBrowser();
        }

        internal void Run()
        {
            _trayApp = new TrayApp();
            Cmd<Start>.Create().RunAsync();
            Application.Run(_trayApp);
        }

        internal void Exit()
        {
            PlaydarBrowser.CloseOnExit();
            Application.Exit();
        }

        internal void ShowInfoDialog(string msg)
        {
            MessageBox.Show(msg, "Windar Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #region Tray methods

        internal void ShowTrayInfo(string msg)
        {
            ShowTrayInfo(msg, ToolTipIcon.Info);
        }

        internal void ShowTrayInfo(string msg, ToolTipIcon icon)
        {
            if (!Properties.Settings.Default.ShowBalloons) return;
            _trayApp.TrayIcon.Visible = true;
            _trayApp.TrayIcon.ShowBalloonTip(3, "Windar", msg, icon);
        }

        #endregion

        #region Uninstaller Shutdown File Watcher

        private void SetupShutdownFileWatcher()
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

        private void ShutdownFile_OnChanged(object source, FileSystemEventArgs e)
        {
            if (Log.IsInfoEnabled) Log.Info("Shutdown initiated by the file-system event.");
            _trayApp.Stop(null, null);
        }

        #endregion
    }
}
