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
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using log4net;
using Windar.TrayApp.Configuration;

namespace Windar.TrayApp
{
    partial class MainForm : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        private bool _reallyClose;
        private bool _resizing;
        private string _lastLink;
        private TabPage _lastSelectedTab;
        private FormWindowState _lastWindowState;
        private Size _oldSize;
        private IOptionsPage _optionsPage;

        #region Init

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Version info for the About page.
            var info = new StringBuilder();
            info.Append(Program.AssemblyProduct).Append(' ').Append(Program.AssemblyVersion);
            versionLabel.Text = info.ToString();

            RestoreWindowLayout();
            GoToAboutPage();

#if DEBUG
            logBox.Load();
            logBox.VScroll += RichTextBoxPlus_VScroll;
            logBox.ScrollToEnd();
#else
            MainTabControl.TabPages.Remove(logBoxTab);
            _logBoxTab = null;
            _logBox = null;
#endif

            //TODO: Re-add tabs.
            optionsTabControl.TabPages.Remove(scriptsTabPage);
            optionsTabControl.TabPages.Remove(pluginsTabPage);
            optionsTabControl.TabPages.Remove(propsTabPage);

            LoadPlaydarHomepage();
        }

        private void RichTextBoxPlus_VScroll(object sender, EventArgs e)
        {
            if (logBox != null)
            {
                Program.Instance.MainForm.followTailCheckBox.Checked = logBox.FollowTail;
            }
        }

        #endregion

        public void EnsureVisible()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = _lastWindowState;
            }
            if (!Visible) Show();
            Program.Instance.MainForm.Activate();
            if (Properties.Settings.Default.MainFormVisible) return;
            Properties.Settings.Default.MainFormVisible = true;
            Properties.Settings.Default.Save();
        }

        public void GoToAboutPage()
        {
            var page = MainTabControl.TabPages["aboutTabPage"];
            MainTabControl.SelectTab(page);
        }

        public void GoToPlaydarHomePage()
        {
            LoadPlaydarHomepage();
            if (MainTabControl.SelectedTab != playdarTabPage)
            {
                MainTabControl.SelectTab(playdarTabPage);
            }
        }

        #region Playdar daemon browser.

        internal void LoadPlaydarHomepage()
        {
            if (PlaydarBrowser.Document == null
                || PlaydarBrowser.Document.Url == null
                || !PlaydarBrowser.Document.Url.Equals(Program.PlaydarDaemon))
            {
                PlaydarBrowser.Navigate(Program.PlaydarDaemon);
            }
        }

        private void startDaemonButton_Click(object sender, EventArgs e)
        {
            Program.Instance.StartDaemon();
        }

        private void stopDaemonButton_Click(object sender, EventArgs e)
        {
            Program.Instance.StopDaemon();
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            Program.Instance.RestartDaemon();
            PlaydarBrowser.Refresh();
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            if (Program.Instance.Daemon.Started)
            {
                LoadPlaydarHomepage();
            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            if (Program.Instance.Daemon.Started)
            {
                PlaydarBrowser.GoBack();
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            if (Program.Instance.Daemon.Started)
            {
                PlaydarBrowser.Refresh();
            }
        }

        private void playdarLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.playdar.org/");
        }

        private void playdarBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            var url = e.Url.ToString();
            if (url.Equals(Program.PlaydarDaemon))
            {
                homeButton.Enabled = false;
                backButton.Enabled = false;
                return;
            }
            if (url.StartsWith(Program.PlaydarDaemon))
            {
                homeButton.Enabled = true;
                backButton.Enabled = true;
                return;
            }
            if (url.StartsWith("res://ieframe.dll/navcancl.htm"))
            {
                e.Cancel = true;
            }
            else if (url.Equals("about:blank"))
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
                System.Diagnostics.Process.Start(url);
            }
        }

        private void playdarBrowser_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            if (!_lastLink.StartsWith(Program.PlaydarDaemon))
            {
                PlaydarBrowser.Navigate(_lastLink);
            }
            else
            {
                System.Diagnostics.Process.Start(_lastLink);
            }
        }

        private void PlaydarBrowserLinkClicked(object sender, EventArgs e)
        {
            if (PlaydarBrowser.Document == null) return;
            var link = PlaydarBrowser.Document.ActiveElement;
            if (link == null) return;
            _lastLink = link.GetAttribute("href");
        }

        private void playdarBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (PlaydarBrowser.Document == null) return;
            var links = PlaydarBrowser.Document.Links;
            foreach (HtmlElement var in links)
            {
                var.AttachEventHandler("onclick", PlaydarBrowserLinkClicked);
            }
        }

        #endregion

        private void followTailCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (logBox == null) return;
            logBox.FollowTail = followTailCheckBox.Checked;
            if (logBox.FollowTail) logBox.ScrollToEnd();
        }

        #region Closing

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MainTabControl.SelectedTab == optionsTabPage)
            {
                e.Cancel = !ApplyOptionsOrCancel();
                if (e.Cancel) return;
            }
            if (_reallyClose) return;
            e.Cancel = true;
            Hide();
            Properties.Settings.Default.MainFormVisible = false;
            Properties.Settings.Default.Save();
        }

        internal void Exit()
        {
            if (logBox != null) logBox.Close();
            _reallyClose = true;
            Close();
        }

        #endregion

        #region Window layout persistence.

        private void PersistWindowLayout()
        {
            switch (WindowState)
            {
                case FormWindowState.Normal:
                    Properties.Settings.Default.MainFormWindowMaximised = false;
                    Properties.Settings.Default.MainFormWindowSize = Size;
                    Properties.Settings.Default.MainFormWindowLocation = Location;
                    break;
                case FormWindowState.Maximized:
                    Properties.Settings.Default.MainFormWindowMaximised = true;
                    Properties.Settings.Default.MainFormWindowSize = RestoreBounds.Size;
                    Properties.Settings.Default.MainFormWindowLocation = RestoreBounds.Location;
                    break;
                default:
                    Properties.Settings.Default.MainFormWindowMaximised = false;
                    Properties.Settings.Default.MainFormWindowSize = RestoreBounds.Size;
                    Properties.Settings.Default.MainFormWindowLocation = RestoreBounds.Location;
                    break;
            }
            if (Log.IsDebugEnabled) Log.Debug("Persisting window layout.");
            Properties.Settings.Default.Save();
        }

        private void RestoreWindowLayout()
        {
            // Window location.
            if (Properties.Settings.Default.MainFormWindowLocation == new Point(0, 0))
            {
                StartPosition = FormStartPosition.CenterScreen;
                Size = new Size(640, 480);
            }
            else
            {
                Location = Properties.Settings.Default.MainFormWindowLocation;
                Size = Properties.Settings.Default.MainFormWindowSize;
            }

            // Maximised state.
            if (Properties.Settings.Default.MainFormWindowMaximised)
            {
                WindowState = FormWindowState.Maximized;
            }

            // Fit to screen if necessary.
            if (Size.Width > Screen.PrimaryScreen.WorkingArea.Width
                || Size.Height > Screen.PrimaryScreen.WorkingArea.Height)
            {
                if (Log.IsWarnEnabled) Log.Warn("Window size exceeds screen area.");
                if (Log.IsInfoEnabled)
                {
                    Log.Info("Width = " + Size.Width + ", Height = " + Size.Height);
                    Log.Info("WorkingArea.Width = " + Screen.PrimaryScreen.WorkingArea.Width +
                             ", WorkingArea.Height = " + Screen.PrimaryScreen.WorkingArea.Height);
                }
                StartPosition = FormStartPosition.CenterScreen;
                Size = new Size(640, 480);
            }
        }

        #endregion

        #region Window movement.

        private void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            _resizing = true;
            _oldSize = Size;
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            if (MainTabControl.SelectedTab == logTabPage
                && logTabPage != null
                && Size != _oldSize
                && logBox != null) logBox.ScrollToEnd();
            _resizing = false;
            PersistWindowLayout();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (_resizing) return;
            if (MainTabControl.SelectedTab == logTabPage
                && logTabPage != null)
            {
                if (logBox != null)
                {
                    if (_lastWindowState == FormWindowState.Maximized)
                    {
                        logBox.ReSetText();
                    }
                    logBox.ScrollToEnd();
                }
            }
            if (WindowState != FormWindowState.Minimized)
            {
                _lastWindowState = WindowState;
            }
            PersistWindowLayout();
        }

        #endregion

        #region Tab control selection changes.

        private void MainTabControl_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (_lastSelectedTab == optionsTabPage) ApplyOptionsOrCancel();
        }

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Remember last selected tab.
            _lastSelectedTab = MainTabControl.SelectedTab;

            // Check the new selected tab.
            if (MainTabControl.SelectedTab == logTabPage
                && logTabPage != null)
            {
                if (logBox != null)
                {
                    logBox.Visible = true;
                    logBox.Updating = true;
                }
                followTailCheckBox.Checked = true;
                if (logBox != null)
                {
                    logBox.ReSetText();
                    logBox.ScrollToEnd();
                    logBox.Visible = true;
                }
            }
            else
            {
                if (logBox != null) logBox.Updating = false;
                if (MainTabControl.SelectedTab == playdarTabPage)
                {
                    GoToPlaydarHomePage();
                }
                if (MainTabControl.SelectedTab == optionsTabPage)
                {
                    optionsTabControl.SelectTab(generalOptionsTabPage);
                    LoadGeneralOptionsTabPage();
                }
            }
        }

        private void optionsTabControl_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            e.Cancel = !ApplyOptionsOrCancel();
        }

        private void optionsTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (optionsTabControl.SelectedTab == generalOptionsTabPage)
            {
                LoadGeneralOptionsTabPage();
            }
            else if (optionsTabControl.SelectedTab == modsTabPage)
            {
                _optionsPage = new PlaydarModulesPage();
                _optionsPage.Load();
            }
            else if (optionsTabControl.SelectedTab == scriptsTabPage)
            {
                _optionsPage = new ResolverScriptsPage();
                _optionsPage.Load();
            }
            else if (optionsTabControl.SelectedTab == pluginsTabPage)
            {
                _optionsPage = new PluginsPage();
                _optionsPage.Load();
            }
            else if (optionsTabControl.SelectedTab == propsTabPage)
            {
                _optionsPage = new PluginPropertiesPage();
                _optionsPage.Load();
            }
            else
            {
                if (Log.IsWarnEnabled)
                {
                    Log.Warn("Unexpected config tab, " + optionsTabControl.SelectedTab.Name);
                }
            }
        }

        #endregion

        private void LoadGeneralOptionsTabPage()
        {
            _optionsPage = new GeneralOptionsPage();
            _optionsPage.Load();
            nodeNameTextBox.Text = "Test";
            portTextBox.Text = "60210";
        }

        /// <summary>
        /// Check if changes. Require save or cancel changes.
        /// </summary>
        /// <returns>Returns true if ok to proceed, false otherwise.</returns>
        private bool ApplyOptionsOrCancel()
        {
            if (Visible && _optionsPage.Changed)
            {
                var result = Program.ShowYesNoCancelDialog("Save changes?");
                if (result == DialogResult.Cancel) return false;
                if (result == DialogResult.Yes) _optionsPage.SaveChanges();
                _optionsPage.Reset();
            }
            return true;
        }
    }
}
