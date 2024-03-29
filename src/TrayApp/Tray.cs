﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Net;
using System.Text;
using log4net;

namespace Windar.TrayApp
{
    partial class Tray : Form
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        NotifyIcon _notifyIcon;

        // Notification tray icon.
        internal NotifyIcon NotifyIcon
        {
            get { return _notifyIcon; }
            set { _notifyIcon = value; }
        }

        // Tray menu.
        readonly ContextMenu _trayMenu;
        readonly MenuItem _aboutMenuItem;
        readonly MenuItem _updateMenuItem;
        readonly MenuItem _daemonMenuItem;
        readonly MenuItem _playlickMenuItem;
        readonly MenuItem _moreDemosMenuItem;
        readonly MenuItem _balloonsMenuItem;
        readonly MenuItem _scanfilesMenuItem;
        readonly MenuItem _numfilesMenuItem;
        readonly MenuItem _pingMenuItem;
        readonly MenuItem _shutdownMenuItem;
        readonly MenuItem _restartMenuItem;

        #region Init

        public Tray()
        {
            InitializeComponent();

            // Tray menu items.
            _aboutMenuItem = new MenuItem("About Windar", ShowAbout);
            _updateMenuItem = new MenuItem("Check for Updates", CheckForUpdates);
            _daemonMenuItem = new MenuItem("Playdar Information", ShowDaemonInfo);
            _playlickMenuItem = new MenuItem("Playlick", OpenPlaylickWebsite);
            _moreDemosMenuItem = new MenuItem("More Demos", OpenMoreDemos);
            _balloonsMenuItem = new MenuItem("Show Messages", ToggleShowBalloons);
            _balloonsMenuItem.Checked = Properties.Settings.Default.ShowBalloons;
            _scanfilesMenuItem = new MenuItem("Scan Files", ShowScanSelect);
            _numfilesMenuItem = new MenuItem("Number of Files", NumFiles);
            _pingMenuItem = new MenuItem("Ping", Ping);
            _restartMenuItem = new MenuItem("Restart Playdar", Restart);
            _shutdownMenuItem = new MenuItem("Shutdown", Shutdown);

            // Demos menu.
            MenuItem demos = new MenuItem("Playdar Demos");
            demos.MenuItems.Add(_playlickMenuItem);
            demos.MenuItems.Add(_moreDemosMenuItem);

            // Tray menu.
            _trayMenu = new ContextMenu();
            _trayMenu.MenuItems.Add(_aboutMenuItem);
            _trayMenu.MenuItems.Add(_updateMenuItem);
            _trayMenu.MenuItems.Add("-");
            _trayMenu.MenuItems.Add(_daemonMenuItem);
            _trayMenu.MenuItems.Add(demos);
            _trayMenu.MenuItems.Add("-");
            _trayMenu.MenuItems.Add(_scanfilesMenuItem);
            _trayMenu.MenuItems.Add(_numfilesMenuItem);
            _trayMenu.MenuItems.Add(_pingMenuItem);
            _trayMenu.MenuItems.Add("-");
            _trayMenu.MenuItems.Add(_balloonsMenuItem);
            _trayMenu.MenuItems.Add("-");
            _trayMenu.MenuItems.Add(_restartMenuItem);
            _trayMenu.MenuItems.Add(_shutdownMenuItem);

            // Tray icon.
            NotifyIcon = new NotifyIcon();
            NotifyIcon.Text = "Playdar";
            NotifyIcon.Icon = Properties.Resources.trayIcon;
            NotifyIcon.ContextMenu = _trayMenu;
            NotifyIcon.Visible = true;

            // Double-click handler for tray icon.
            NotifyIcon.MouseDoubleClick += TrayIcon_MouseDoubleClick;
            NotifyIcon.MouseClick += TrayIcon_MouseClick;
        }

        protected override void OnLoad(EventArgs e)
        {
            _balloonsMenuItem.Checked = Properties.Settings.Default.ShowBalloons;

            // Hide the form.
            Visible = false;
            ShowInTaskbar = false;

            base.OnLoad(e);
        }

        #endregion

        internal void ToggleMainFormOptions(bool enable)
        {
            _aboutMenuItem.Visible = enable;
            _daemonMenuItem.Visible = enable;
            _scanfilesMenuItem.Visible = enable;
            _restartMenuItem.Visible = enable;
        }

        #region Menu option click handlers.

        #region Playdar controller commands.

        static void Shutdown(object sender, EventArgs e)
        {
            Program.Shutdown();
        }

        static void Restart(object sender, EventArgs e)
        {
            Program.Instance.RestartDaemon();
        }

        static void Ping(object sender, EventArgs e)
        {
            Program.ShowInfoDialog(Program.Instance.Daemon.Ping());
        }

        static void NumFiles(object sender, EventArgs e)
        {
            string result;
            try
            {
                int n = Program.Instance.Daemon.NumFiles;
                if (n == 1) result = "There is one file in your Playdar library.";
                else result = "There are " + n + " files in your Playdar library.";
            }
            catch (Exception ex)
            {
                result = "Error: " + ex.Message;
            }
            Program.ShowInfoDialog(result);
        }

        #endregion

        #region Dialogs

        static void ShowAbout(object sender, EventArgs e)
        {
            Program.Instance.MainForm.GoToAboutPage();
        }

        static void ShowDaemonInfo(object sender, EventArgs e)
        {
            Program.Instance.MainForm.GoToPlaydarHomePage();
        }

        static void ShowScanSelect(object sender, EventArgs e)
        {
            Program.Instance.MainForm.GoToAddScanFolder();
        }

        #endregion

        static void CheckForUpdates(object sender, EventArgs e)
        {
            try
            {
                // Get the version file from the windar.org website.
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://windar.org/latest/version");
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                Stream stream = response.GetResponseStream();
                byte[] buf = new byte[8192];
                StringBuilder sb = new StringBuilder();
                int count;
                do
                {
                    count = stream.Read(buf, 0, buf.Length);
                    if (count == 0) continue;
                    string str = Encoding.ASCII.GetString(buf, 0, count);
                    sb.Append(str);
                }
                while (count > 0);

                // Check if latest version is newer, older or same.
                int latest = ConvertVersionString(sb.ToString().Split('.'));
                int installed = ConvertVersionString(Program.AssemblyVersion.Split('.'));
                if (latest > installed)
                {
                    StringBuilder msg = new StringBuilder();
                    msg.Append("There is a new version available!\n");
                    msg.Append("Go to download website now?");
                    if (Program.ShowYesNoDialog(msg.ToString()))
                    {
                        Process.Start("http://windar.org/download/");
                    }

                    //TODO: Download and install automatically?
                }
                else
                {
                    Program.ShowInfoDialog("You have the latest version.");
                }
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled) Log.Error("Exception when checking for latest version.", ex);
                Program.ShowErrorDialog(ex);
            }
        }

        void ToggleShowBalloons(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowBalloons = !Properties.Settings.Default.ShowBalloons;
            Properties.Settings.Default.Save();
            _balloonsMenuItem.Checked = Properties.Settings.Default.ShowBalloons;
        }

        #region Web links

        static void OpenPlaylickWebsite(object sender, EventArgs e)
        {
            Process.Start("http://www.playlick.com/");
        }

        static void OpenPlaygrub(object sender, EventArgs e)
        {
            Process.Start("http://playgrub.com/");
        }

        static void OpenSpiffdar(object sender, EventArgs e)
        {
            Process.Start("http://spiffdar.playnode.org/");
        }

        static void OpenMoreDemos(object sender, EventArgs e)
        {
            Process.Start("http://www.playdar.org/demos/");
        }

        #endregion

        #endregion

        #region Tray icon mouse clicks.

        static void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!Program.Instance.MainForm.Visible) Program.Instance.MainForm.GoToAboutPage();
            else Program.Instance.MainForm.EnsureVisible();
        }

        static void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Program.Instance.MainForm.Visible)
                Program.Instance.MainForm.EnsureVisible();
        }

        #endregion

        /// <summary>
        /// This method is used to convert version to comparable strings.
        /// The version string is converted to an integer, allowing 2 digits for each of the
        /// major, minor and build components. 4 digits are allowed for the revision.
        /// </summary>
        /// <param name="version">Version as split string.</param>
        /// <returns>Version as integer value.</returns>
        static int ConvertVersionString(string[] version)
        {
            StringBuilder strBuild = new StringBuilder();
            strBuild.Append(version[0].PadLeft(2, '0')); // Major
            strBuild.Append(version[1].PadLeft(2, '0')); // Minor
            strBuild.Append(version[2].PadLeft(2, '0')); // Build
            strBuild.Append(version[3].PadLeft(4, '0')); // Revision
            return Int32.Parse(strBuild.ToString());
        }
    }
}
