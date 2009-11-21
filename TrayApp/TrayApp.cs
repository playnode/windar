﻿/*
 * Windar: Playdar for Windows
 * Copyright (C) 2009 Steven Robertson <http://stever.org.uk/>
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
using System.Text;
using System.Windows.Forms;
using Windar.Commands;

namespace Windar
{
    public partial class TrayApp : Form
    {
        // Notification tray icon.
        internal NotifyIcon TrayIcon { get; private set; }

        // Tray menu.
        private readonly ContextMenu _trayMenu;
        private readonly MenuItem _aboutMenuItem;
        private readonly MenuItem _updateMenuItem;
        private readonly MenuItem _daemonMenuItem;
        private readonly MenuItem _spiffdarMenuItem;
        private readonly MenuItem _playgrubMenuItem;
        private readonly MenuItem _searchMenuItem;
        private readonly MenuItem _playlickMenuItem;
        private readonly MenuItem _balloonsMenuItem;
        private readonly MenuItem _scanfilesMenuItem;
        private readonly MenuItem _numfilesMenuItem;
        private readonly MenuItem _pingMenuItem;
        private readonly MenuItem _shutdownMenuItem;
        private readonly MenuItem _restartMenuItem;
        private readonly MenuItem _testOptionMenuItem;

        #region Init

        public TrayApp()
        {
            InitializeComponent();

            // Tray menu items.
            _aboutMenuItem = new MenuItem("About Windar", ShowAbout);
            _updateMenuItem = new MenuItem("Check for Updates", CheckForUpdates) { Enabled = false };
            _daemonMenuItem = new MenuItem("Playdar Daemon Info", ShowDaemonInfo);
            _playgrubMenuItem = new MenuItem("Playgrub", OpenPlaygrub);
            _spiffdarMenuItem = new MenuItem("Spiffdar", OpenSpiffdarWebsite);
            _searchMenuItem = new MenuItem("Search", OpenSearchWebsite);
            _playlickMenuItem = new MenuItem("Playlick", OpenPlaylickWebsite);
            _balloonsMenuItem = new MenuItem("Show Balloons", ToggleShowBalloons) {Checked = Properties.Settings.Default.ShowBalloons };
            _scanfilesMenuItem = new MenuItem("Scan Files", Scan);
            _numfilesMenuItem = new MenuItem("Number of Files", NumFiles);
            _pingMenuItem = new MenuItem("Ping", Ping);
            _shutdownMenuItem = new MenuItem("Shutdown", Shutdown);
            _restartMenuItem = new MenuItem("Restart", Restart);
            _testOptionMenuItem = new MenuItem("Test", Test);

            // Demos menu.
            var demos = new MenuItem("Playdar Demos");
            demos.MenuItems.Add(_searchMenuItem);
            demos.MenuItems.Add(_playlickMenuItem);
            demos.MenuItems.Add(_playgrubMenuItem);
            demos.MenuItems.Add(_spiffdarMenuItem);

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
#if DEBUG
            _trayMenu.MenuItems.Add("-");
            _trayMenu.MenuItems.Add(_testOptionMenuItem);
#endif

            // Tray icon.
            TrayIcon = new NotifyIcon
            {
                Text = "Playdar",
                Icon = Properties.Resources.Playdar,
                ContextMenu = _trayMenu,
                Visible = true
            };

            // Double-click handler for tray icon.
            TrayIcon.MouseDoubleClick += TrayIcon_MouseDoubleClick;
            TrayIcon.MouseClick += TrayIcon_MouseClick;
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

        #region Playdar controller commands

        private static void Shutdown(object sender, EventArgs e)
        {
            Program.Instance.Shutdown();
        }

        private static void Restart(object sender, EventArgs e)
        {
            Program.Instance.RestartDaemon();
        }

        private static void Ping(object sender, EventArgs e)
        {
            Program.Instance.ShowInfoDialog(Cmd<Ping>.Create().Run());
        }

        private static void NumFiles(object sender, EventArgs e)
        {
            Program.Instance.ShowInfoDialog(Cmd<NumFiles>.Create().Run());
        }

        private void Scan(object sender, EventArgs e)
        {
            var dialog = new DirectoryDialog
                             {
                                 BrowseFor = DirectoryDialog.BrowseForTypes.FilesAndDirectories,
                                 Title = "Select a file or a folder"
                             };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            var filename = dialog.Selected;
            var cmd = Cmd<Scan>.Create();            
            Program.Instance.ShowTrayInfo("Scanning in progress.");
            cmd.RunAsync(filename);
        }

        #endregion

        #region Dialogs

        private static void ShowAbout(object sender, EventArgs e)
        {
            Program.Instance.MainForm.GoToAbout();
            Program.Instance.MainForm.EnsureVisible();
        }

        private static void ShowDaemonInfo(object sender, EventArgs e)
        {
            Program.Instance.MainForm.GoToPlaydarDaemon();
            Program.Instance.MainForm.EnsureVisible();
        }

        #endregion

        private static void CheckForUpdates(object sender, EventArgs e)
        {
            //TODO: CheckForUpdates
        }

        private void ToggleShowBalloons(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowBalloons = !Properties.Settings.Default.ShowBalloons;
            _balloonsMenuItem.Checked = Properties.Settings.Default.ShowBalloons;

            // NOTE: Save after changing settings as potentially no graceful shutdown occurs.
            Properties.Settings.Default.Save();
        }

        #region Web links

        private static void OpenPlaygrub(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://playgrub.com/");
        }

        private static void OpenSpiffdarWebsite(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://spiffdar.org/");
        }

        private static void OpenPlaylickWebsite(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.playlick.com/");
        }

        private static void OpenSearchWebsite(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.playdar.org/demos/search.html");
        }

        #endregion

        private static void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Program.Instance.MainForm.EnsureVisible();
        }

        private static void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Program.Instance.MainForm.Visible)
            {
                Program.Instance.MainForm.EnsureVisible();
            }
        }

        private static void Test(object sender, EventArgs e)
        {
            var track = new Track("Artemis", "Hypno");
            var similar = track.GetRecommendations();
            var strBuild = new StringBuilder();
            foreach (var similarTrack in similar)
            {
                strBuild.Append(similarTrack.Artist);
                strBuild.Append(" - ");
                strBuild.Append(similarTrack.Title);
                strBuild.Append('\n');
            }
            MessageBox.Show(strBuild.ToString(), "Recommendations", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}