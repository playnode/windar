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
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using log4net;
using Windar.TrayApp.Configuration;

namespace Windar.TrayApp
{
    partial class MainForm : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern long DeleteUrlCacheEntry(string lpszUrlName);

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
            optionsTabControl.TabPages.Remove(pluginsTabPage);
            optionsTabControl.TabPages.Remove(propsTabPage);

            ShowDaemonPage();
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
                || !PlaydarBrowser.Document.Url.Equals(Program.Instance.PlaydarDaemon))
            {
                PlaydarBrowser.Navigate(Program.Instance.PlaydarDaemon);
            }
        }

        internal void ShowDaemonPage()
        {
            ShowDaemonPage(null);
        }

        internal void ShowDaemonPage(string text)
        {
            if (string.IsNullOrEmpty(text)) text = "&nbsp;";
            PlaydarBrowser.Stop();
            var html = new StringBuilder();
            html.Append("<html>").Append(Environment.NewLine);
            html.Append("<head>").Append(Environment.NewLine);
            html.Append("<style>").Append(Environment.NewLine);
            html.Append("body { font-family: verdana; }").Append(Environment.NewLine);
            html.Append("</style>").Append(Environment.NewLine);
            html.Append("</head>").Append(Environment.NewLine);
            html.Append("<body bgcolor=\"");
            html.Append(ColorTranslator.ToHtml(Color.FromKnownColor(KnownColor.ControlLight)));
            html.Append("\">").Append(Environment.NewLine);
            html.Append(text).Append(Environment.NewLine);
            html.Append("</body></html>");
            if (PlaydarBrowser.Document != null) 
            {
                PlaydarBrowser.Document.OpenNew(true);
                PlaydarBrowser.Document.Write(html.ToString());
            }
            else
            {
                PlaydarBrowser.DocumentText = html.ToString();
            }
            Application.DoEvents();
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
            if (Program.Instance.Daemon.Started) PlaydarBrowser.Refresh();
        }

        private void playdarLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.playdar.org/");
        }

        private void playdarBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            var url = e.Url.ToString();
            if (Log.IsDebugEnabled) Log.Debug("Navigating to " + url);

            // Make sure the page isn't from the cache!
            DeleteUrlCacheEntry(url);

            // Handle some expected pages.
            if (url.Equals(Program.Instance.PlaydarDaemon))
            {
                HomeButton.Enabled = false;
                BackButton.Enabled = false;
                return;
            }
            if (url.StartsWith(Program.Instance.PlaydarDaemon))
            {
                HomeButton.Enabled = true;
                BackButton.Enabled = true;
                return;
            }
            if (url.StartsWith("res://ieframe.dll/navcancl.htm"))
            {
                ShowDaemonPage("Error");
                e.Cancel = true;
            }
            else if (url.Equals("about:blank"))
            {
                ShowDaemonPage();
                e.Cancel = true;
            }
            else
            {
                System.Diagnostics.Process.Start(url);
                e.Cancel = true;
            }
        }

        private void playdarBrowser_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            if (!_lastLink.StartsWith(Program.Instance.PlaydarDaemon))
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
            var url = e.Url.ToString();
            if (Log.IsDebugEnabled) Log.Debug("Document completed. " + url);

            if (PlaydarBrowser.Document == null) return;

            if (PlaydarBrowser.Document.Title == "Internet Explorer cannot display the webpage")
            {
                RefreshButton.Enabled = false;
                if (url.Equals(Program.Instance.PlaydarDaemon))
                {
                    StartDaemonButton.Enabled = true;
                    RestartDaemonButton.Enabled = false;
                    StopDaemonButton.Enabled = false;
                    ShowDaemonPage("Playdar service not running.");
                }
                else
                {
                    ShowDaemonPage("Page not available.");
                }
                return;
            }

            // Attach click event handler to all javascript links too.
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
            if (MainTabControl.SelectedTab == optionsTabPage
                && (e.Cancel = !ApplyOptionsOrCancel())) return;
            if (_reallyClose) return;
            if (!Program.Instance.Daemon.Started) Program.Shutdown();
            else
            {
                e.Cancel = true;
                Hide();
                Properties.Settings.Default.MainFormVisible = false;
                Properties.Settings.Default.Save();
            }
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
            if (_lastSelectedTab == optionsTabPage) e.Cancel = !ApplyOptionsOrCancel();
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
                    logBoxPanel.Visible = false;
                    logBox.ReSetText();
                    logBox.ScrollToEnd();
                    logBoxPanel.Visible = true;
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
                    SetupGeneralOptionsPage();
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
                SetupGeneralOptionsPage();
            }
            else if (optionsTabControl.SelectedTab == libraryTabPage)
            {
                SetupLibraryPage();
            }
            else if (optionsTabControl.SelectedTab == modsTabPage)
            {
                SetupModulesPage();
            }
            else if (optionsTabControl.SelectedTab == pluginsTabPage)
            {
                SetupPluginsPage();
            }
            else if (optionsTabControl.SelectedTab == propsTabPage)
            {
                SetupPluginsPropertiesPage();
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
                if (result == DialogResult.Yes)
                {
                    if (_optionsPage is GeneralOptionsPage) SaveGeneralOptions();
                }
            }
            return true;
        }

        private static void ShowApplyChangesDialog()
        {
            Program.Instance.SaveConfiguration();
            if (Program.ShowYesNoDialog("Restart Playdar to apply changes?"))
                Program.Instance.Daemon.Restart();
        }

        #region General options page.

        private void SetupGeneralOptionsPage()
        {
            Program.Instance.ReloadConfiguration();
            GeneralOptionsPage options;
            _optionsPage = options = new GeneralOptionsPage();
            _optionsPage.Load();

            nodeNameTextBox.Text = options.NodeName;
            portTextBox.Text = options.Port.ToString();
            allowIncomingCheckBox.Checked = options.AllowIncoming;
            forwardCheckBox.Checked = options.ForwardQueries;
            //TODO: Autostart

            LoadPeers();
        }

        private void SaveGeneralOptions()
        {
            foreach (var obj in peersGrid.Rows)
            {
                var row = (DataGridViewRow) obj;

                // Get host, port and share from row.
                var host = (string) row.Cells[0].Value;
                var port = 0;
                var portObj = row.Cells[1].Value;
                if (portObj is int) port = (int) portObj;
                else if (portObj is string)
                {
                    try
                    {
                        port = Int32.Parse((string) row.Cells[1].Value);
                    }
                    catch (FormatException ex)
                    {
                        if (Log.IsErrorEnabled)
                        {
                            Log.Error("Couldn't parse \"" + (string) row.Cells[1].Value + "\" as integer.", ex);
                        }
                    }
                }
                else
                {
                    if (Log.IsWarnEnabled)
                    {
                        Log.Warn("Unexpected type for port. portObj = " + portObj);
                    }
                }
                var share = (bool) row.Cells[2].Value;

                // Handle new and updated rows.
                if (row.Tag != null)
                {
                    // Updating
                    var tag = (PeerInfo) row.Tag;
                    if (host != tag.Host || port != tag.Port)
                    {
                        ((GeneralOptionsPage) _optionsPage).RemovePeer(tag.Host, tag.Port);
                        ((GeneralOptionsPage) _optionsPage).AddNewPeer(host, port, share);
                    }
                }
                else
                {
                    // Adding
                    ((GeneralOptionsPage) _optionsPage).AddNewPeer(host, port, share);
                }
            }
            Program.Instance.SaveConfiguration();
            SetupGeneralOptionsPage();
            UpdateGeneralOptionsButtons();
            ShowApplyChangesDialog();
        }

        private void generalOptionsCancelButton_Click(object sender, EventArgs e)
        {
            SetupGeneralOptionsPage();
            generalOptionsSaveButton.Enabled = _optionsPage.Changed;
            generalOptionsCancelButton.Enabled = _optionsPage.Changed;
        }

        private void generalOptionsSaveButton_Click(object sender, EventArgs e)
        {
            SaveGeneralOptions();
        }

        #region Change handling.

        private void UpdateGeneralOptionsButtons()
        {
            generalOptionsSaveButton.Enabled = _optionsPage.Changed;
            generalOptionsCancelButton.Enabled = _optionsPage.Changed;
        }

        private void allowIncomingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ((GeneralOptionsPage) _optionsPage).AllowIncoming = allowIncomingCheckBox.Checked;
            UpdateGeneralOptionsButtons();
        }

        private void autostartCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ((GeneralOptionsPage) _optionsPage).AutoStart = autostartCheckBox.Checked;
            UpdateGeneralOptionsButtons();
        }

        private void forwardCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ((GeneralOptionsPage) _optionsPage).ForwardQueries = forwardCheckBox.Checked;
            UpdateGeneralOptionsButtons();
        }

        private void nodeNameTextBox_TextChanged(object sender, EventArgs e)
        {
            ((GeneralOptionsPage) _optionsPage).NodeName = nodeNameTextBox.Text;
            UpdateGeneralOptionsButtons();
        }

        private void portTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ((GeneralOptionsPage) _optionsPage).Port = Int32.Parse(portTextBox.Text);
                UpdateGeneralOptionsButtons();
            }
            catch (FormatException ex)
            {
                if (Log.IsErrorEnabled) Log.Error("Exception", ex);
                portTextBox.Text = "";
            }
        }

        // NOTE: Changes to peers are handled on add and remove rows.

        #endregion

        #region Peers

        private void LoadPeers()
        {
            peersGrid.Rows.Clear();
            foreach (var peer in ((GeneralOptionsPage) _optionsPage).Peers)
                peersGrid.Rows.Add(GetPeerListRow(peer));
            if (peersGrid.Rows.Count <= 0) return;
            peersGrid.Rows[0].Selected = false;
            peersGrid.CurrentCell = null;
        }

        private static DataGridViewRow GetPeerListRow(PeerInfo peer)
        {
            var row = new DataGridViewRow();
            row.Cells.Add(new DataGridViewTextBoxCell { Value = peer.Host });
            row.Cells.Add(new DataGridViewTextBoxCell { Value = peer.Port });
            row.Cells.Add(new DataGridViewCheckBoxCell { Value = peer.Share });
            row.Tag = peer;
            return row;
        }

        private static DataGridViewRow GetPeerListRow()
        {
            var row = new DataGridViewRow();
            row.Cells.Add(new DataGridViewTextBoxCell { Value = "" });
            row.Cells.Add(new DataGridViewTextBoxCell { Value = "60211" });
            row.Cells.Add(new DataGridViewCheckBoxCell { Value = false });
            return row;
        }

        private void peersGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (peersGrid.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.Cell) return;
            foreach (var obj in peersGrid.SelectedRows) ((DataGridViewRow) obj).Selected = false;
            peersGrid.CurrentCell = null;
        }

        private void peersGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            peersContextMenu.Items["removePeerMenuItem"].Visible = peersGrid.SelectedRows.Count > 0;
        }

        private void addPeerMenuItem_Click(object sender, EventArgs e)
        {
            peersGrid.Rows.Add(GetPeerListRow());
            ((GeneralOptionsPage) _optionsPage).NewPeersToAdd = true;
            UpdateGeneralOptionsButtons();
            
            // Select the first cell the new row.
            var row = peersGrid.Rows[peersGrid.Rows.Count - 1];
            peersGrid.CurrentCell = row.Cells[0];
            peersGrid.BeginEdit(false);
        }

        private void removePeerMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var obj in peersGrid.SelectedRows)
            {
                var row = (DataGridViewRow) obj;
                if (row.Tag != null && row.Tag is PeerInfo)
                {
                    var peer = (PeerInfo) row.Tag;
                    ((GeneralOptionsPage) _optionsPage).RemovePeer(peer.Host, peer.Port);
                }
                peersGrid.Rows.Remove(row);
            }
            UpdateGeneralOptionsButtons();
        }

        private void peersGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (peersGrid.Rows.Count <= 0 || peersGrid.Rows[e.RowIndex].Tag == null) return;
            ((GeneralOptionsPage) _optionsPage).PeerValueChanged = true;
            UpdateGeneralOptionsButtons();
        }

        #endregion

        #endregion

        private void SetupLibraryPage()
        {
            LibraryOptionsPage options;
            _optionsPage = options = new LibraryOptionsPage();
            _optionsPage.Load();
        }

        private void SetupModulesPage()
        {
            PlaydarModulesPage options;
            _optionsPage = options = new PlaydarModulesPage();
            _optionsPage.Load();
        }

        private void SetupPluginsPage()
        {
            PluginsPage options;
            _optionsPage = options = new PluginsPage();
            _optionsPage.Load();
        }

        private void SetupPluginsPropertiesPage()
        {
            PluginPropertiesPage options;
            _optionsPage = options = new PluginPropertiesPage();
            _optionsPage.Load();
        }

        private void libraryGrid_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void modsGrid_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void pluginsGrid_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void propsGrid_MouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}
