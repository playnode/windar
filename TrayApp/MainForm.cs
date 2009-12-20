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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using log4net;
using Windar.Common;
using Windar.TrayApp.Configuration;

namespace Windar.TrayApp
{
    partial class MainForm : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        internal delegate void ShowTracklistCallback(string text, int n);

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern long DeleteUrlCacheEntry(string lpszUrlName);

        private bool _exiting;
        private bool _resizing;
        private bool _inDirectoryDialog;
        private string _lastLink;
        private TabPage _lastSelectedTab;
        private FormWindowState _lastWindowState;
        private Size _oldSize;
        private IOptionsPage _optionsPage;
        private int _navLoopCount;

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
            optionsTabControl.TabPages.Remove(modsTabPage);
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
                WindowState = _lastWindowState;
            if (!Visible) Show();
            Program.Instance.MainForm.Activate();
            if (Properties.Settings.Default.MainFormVisible) return;
            Properties.Settings.Default.MainFormVisible = true;
            Properties.Settings.Default.Save();
        }

        public bool InModalDialog()
        {
            var modal = _inDirectoryDialog;
            if (modal) EnsureVisible();
            return modal;
        }

        #region Navigation methods called from outside MainForm.

        public void GoToAboutPage()
        {
            if (InModalDialog()) return;
            if (MainTabControl.SelectedTab != aboutTabPage)
            {
                if (!Program.Instance.MainForm.Visible)
                {
                    Program.Instance.MainForm.Opacity = 0;
                    MainTabControl.SelectTab(aboutTabPage);
                    Program.Instance.MainForm.Opacity = 1;
                }
                else
                {
                    MainTabControl.SelectTab(aboutTabPage);
                }
            }
            Program.Instance.MainForm.EnsureVisible();
        }

        public void GoToPlaydarHomePage()
        {
            if (InModalDialog()) return;
            if (MainTabControl.SelectedTab != playdarTabPage)
            {
                if (!Program.Instance.MainForm.Visible)
                {
                    Program.Instance.MainForm.Opacity = 0;
                    MainTabControl.SelectTab(playdarTabPage);
                    Program.Instance.MainForm.Opacity = 1;
                }
                else
                {
                    MainTabControl.SelectTab(playdarTabPage);
                }
            }
            Program.Instance.MainForm.EnsureVisible();
            LoadPlaydarHomepage();
        }

        public void GoToAddScanFolder()
        {
            if (InModalDialog()) return;
            if (MainTabControl.SelectedTab != optionsTabPage)
            {
                if (!Program.Instance.MainForm.Visible)
                {
                    Program.Instance.MainForm.Opacity = 0;
                    MainTabControl.SelectTab(optionsTabPage);
                    Program.Instance.MainForm.Opacity = 1;
                }
                else
                {
                    MainTabControl.SelectTab(optionsTabPage);
                }
            }
            if (optionsTabControl.SelectedTab != libraryTabPage)
            {
                if (!Program.Instance.MainForm.Visible)
                {
                    Program.Instance.MainForm.Opacity = 0;
                    optionsTabControl.SelectTab(libraryTabPage);
                    Program.Instance.MainForm.Opacity = 1;
                }
                else
                {
                    optionsTabControl.SelectTab(libraryTabPage);
                }
            }
            Program.Instance.MainForm.EnsureVisible();
            InitialiseLibraryPage();
            AddScanPath();
        }

        #endregion

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
            ShowDaemonPage(null, false);
        }

        internal void ShowDaemonPage(string text, bool newPage)
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
                PlaydarBrowser.Document.OpenNew(!newPage);
                PlaydarBrowser.Document.Write(html.ToString());
            }
            else
            {
                PlaydarBrowser.DocumentText = html.ToString();
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

            // Protect from loop if Internet Explorer is going haywire.
            if (!url.Equals("about:blank")) _navLoopCount = 0;
            else if (_navLoopCount++ > 2) return;

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
                ShowDaemonPage("Error", true);
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

        private void playdarBrowser_NewWindow(object sender, CancelEventArgs e)
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

            if (Log.IsDebugEnabled)
            {
                var msg = new StringBuilder();
                msg.Append("Document completed. URL = \"").Append(url).Append('"');
                if (PlaydarBrowser.Document != null)
                {
                    msg.Append(", Title = ");
                    msg.Append(PlaydarBrowser.Document.Title);
                }
                Log.Debug(msg.ToString());
            }

            RefreshButton.Enabled = true;
            if (url.Equals(Program.Instance.PlaydarDaemon))
            {
                BackButton.Enabled = HomeButton.Enabled = false;
            }
            else
            {
                BackButton.Enabled = PlaydarBrowser.CanGoBack;
                HomeButton.Enabled = true;
            }

            if (PlaydarBrowser.Document == null) return;

            if (PlaydarBrowser.Document.Title == "Internet Explorer cannot display the webpage")
            {
                RefreshButton.Enabled = false;
                if (url.Equals(Program.Instance.PlaydarDaemon))
                {
                    StartDaemonButton.Enabled = true;
                    RestartDaemonButton.Enabled = false;
                    StopDaemonButton.Enabled = false;
                    ShowDaemonPage("Playdar service not running.", false);
                }
                else
                {
                    ShowDaemonPage("Page not available.", false);
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
                && Program.Instance.Config != null
                && (e.Cancel = !ApplyOptionsOrCancel())) return;

            if (_exiting) return;

            if (!Program.Instance.Daemon.Started)
            {
                Program.Shutdown();
            }
            else
            {
                e.Cancel = true;
                Hide();
                MainTabControl.SelectTab(aboutTabPage);
                Properties.Settings.Default.MainFormVisible = false;
                Properties.Settings.Default.Save();
            }
        }

        internal void Exit()
        {
            if (logBox != null) logBox.Close();
            _exiting = true;
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
            if (_lastSelectedTab != optionsTabPage) return;
            if (Program.Instance.Config == null) e.Cancel = true;
            else e.Cancel = !ApplyOptionsOrCancel();
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
                    // Attempt to reload the configuration here.
                    // If it fails the program will be shutdown.
                    if (!Program.Instance.LoadConfiguration()) return;

                    optionsTabControl.SelectTab(generalOptionsTabPage);
                    InitialiseGeneralOptionsPage();
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
                InitialiseGeneralOptionsPage();
            }
            else if (optionsTabControl.SelectedTab == libraryTabPage)
            {
                InitialiseLibraryPage();
            }
            else if (optionsTabControl.SelectedTab == modsTabPage)
            {
                InitialiseModulesPage();
            }
            else if (optionsTabControl.SelectedTab == pluginsTabPage)
            {
                InitialisePluginsPage();
            }
            else if (optionsTabControl.SelectedTab == propsTabPage)
            {
                InitialisePluginsPropertiesPage();
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

        #region Settings pages.

        /// <summary>
        /// Check if changes. Require save or cancel changes.
        /// </summary>
        /// <returns>Returns true if ok to proceed, false otherwise.</returns>
        private bool ApplyOptionsOrCancel()
        {
            if (Visible && _optionsPage != null && _optionsPage.Changed)
            {
                var result = Program.ShowYesNoCancelDialog("Save changes?");
                if (result == DialogResult.Cancel) return false;
                if (result == DialogResult.No) _optionsPage = null;
                else if (_optionsPage is GeneralOptionsPage) SaveGeneralOptions();
                else if (_optionsPage is LibraryOptionsPage) BuildLibrary(false);
                ResetOptionsPagesButtons();
            }
            return true;
        }

        private static void ShowApplyChangesDialog()
        {
            var msg = new StringBuilder();
            msg.Append("To apply changes you will also need to restart Playdar.");
            msg.Append(Environment.NewLine).Append("Do you want to restart Playdar now?");
            if (Program.ShowYesNoDialog(msg.ToString())) Program.Instance.Daemon.Restart();
        }

        private void ResetOptionsPagesButtons()
        {
            UpdateGeneralOptionsButtons();
            UpdateLibraryControls();
        }

        private void cellEndEditTimer_Tick(object sender, EventArgs e)
        {
            if (_optionsPage is GeneralOptionsPage) peersGrid.EndEdit();
            else if (_optionsPage is LibraryOptionsPage) libraryGrid.EndEdit();
            else if (_optionsPage is PlaydarModulesPage) modsGrid.EndEdit();
            else if (_optionsPage is PluginsPage) pluginsGrid.EndEdit();
            else if (_optionsPage is PluginPropertiesPage) propsGrid.EndEdit();
            cellEndEditTimer.Stop();
        }

        #region General options.

        private void InitialiseGeneralOptionsPage()
        {
            GeneralOptionsPage options;
            _optionsPage = options = new GeneralOptionsPage();
            _optionsPage.Load();

            nodeNameTextBox.Text = options.NodeName;
            portTextBox.Text = options.Port.ToString();
            allowIncomingCheckBox.Checked = options.AllowIncoming;
            forwardCheckBox.Checked = options.ForwardQueries;
            //TODO: Autostart option.

            // Load peers table.
            peersGrid.Rows.Clear();
            foreach (var peer in options.Peers) peersGrid.Rows.Add(GetPeerListRow(peer));
            peersGrid.CurrentCell = null;
            if (peersGrid.Rows.Count <= 0) return;
            peersGrid.Rows[0].Selected = false;
        }

        private void UpdateGeneralOptionsButtons()
        {
            var state = _optionsPage != null && _optionsPage is GeneralOptionsPage;
            state = state ? _optionsPage.Changed : false;
            generalOptionsSaveButton.Enabled = state;
            generalOptionsCancelButton.Enabled = state;
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
                    if (host != tag.Host || port != tag.Port || share != tag.Share)
                    {
                        ((GeneralOptionsPage) _optionsPage).RemovePeer(tag.Host, tag.Port);
                        if (!string.IsNullOrEmpty(host) && port > 0)
                            ((GeneralOptionsPage) _optionsPage).AddNewPeer(host, port, share);
                    }
                }
                else
                {
                    // Adding
                    if (!string.IsNullOrEmpty(host) && port > 0)
                        ((GeneralOptionsPage) _optionsPage).AddNewPeer(host, port, share);
                }
            }
            Program.Instance.SaveConfiguration();

            // Attempt to reload the configuration files.
            // Don't continue to apply changes dialog if load fails.
            if (!Program.Instance.LoadConfiguration()) return;

            InitialiseGeneralOptionsPage();
            UpdateGeneralOptionsButtons();
            ShowApplyChangesDialog();
        }

        private void generalOptionsSaveButton_Click(object sender, EventArgs e)
        {
            SaveGeneralOptions();
        }

        private void generalOptionsCancelButton_Click(object sender, EventArgs e)
        {
            Program.Instance.LoadConfiguration();
            InitialiseGeneralOptionsPage();
            generalOptionsSaveButton.Enabled = _optionsPage.Changed;
            generalOptionsCancelButton.Enabled = _optionsPage.Changed;
        }

        #region Change handling.

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

        #endregion

        #region Peers

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
                if (row.Tag != null)
                {
                    var peer = (PeerInfo) row.Tag;
                    ((GeneralOptionsPage) _optionsPage).RemovePeer(peer.Host, peer.Port);
                }
                peersGrid.Rows.Remove(row);
            }
            UpdateGeneralOptionsButtons();
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

        private void peersGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (peersGrid.Rows.Count <= 0 || peersGrid.Rows[e.RowIndex].Tag == null) return;
            ((GeneralOptionsPage) _optionsPage).PeerValueChanged = true;
            UpdateGeneralOptionsButtons();
        }

        private void peersGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            var cell = peersGrid[e.ColumnIndex, e.RowIndex];
            if (cell.GetContentBounds(e.RowIndex).Contains(e.Location)) cellEndEditTimer.Start();
        }

        private void peersGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ((GeneralOptionsPage) _optionsPage).PeerValueChanged = true;
            UpdateGeneralOptionsButtons();
        }

        #endregion

        #endregion

        #region Local library.

        private void InitialiseLibraryPage()
        {
            LibraryOptionsPage options;
            _optionsPage = options = new LibraryOptionsPage();
            _optionsPage.Load();

            // Load scan paths list.
            libraryGrid.Rows.Clear();
            foreach (var path in options.ScanPaths) libraryGrid.Rows.Add(GetScanPathRow(path));

            UpdateLibraryControls();

            // Deselect
            libraryGrid.CurrentCell = null;
            if (libraryGrid.Rows.Count <= 0) return;
            libraryGrid.Rows[0].Selected = false;

            // Sort
            libraryGrid.Sort(libraryGrid.Columns[0], ListSortDirection.Ascending);
        }

        private void SaveLibrarySettings()
        {
            var options = (LibraryOptionsPage) _optionsPage;

            // Update and add paths to scan, as required.
            foreach (var obj in libraryGrid.Rows)
            {
                var row = (DataGridViewRow) obj;
                var path = (string) row.Cells[0].Value;

                // Handle new and updated rows.
                if (row.Tag != null)
                {
                    // Updating
                    var tag = (string) row.Tag;
                    if (path != tag)
                    {
                        options.RemoveScanPath(WindarPaths.ToUnixPath(tag));
                        if (!string.IsNullOrEmpty(path))
                            options.AddScanPath(WindarPaths.ToUnixPath(path));
                    }
                }
                else
                {
                    // Adding
                    if (!string.IsNullOrEmpty(path))
                        options.AddScanPath(WindarPaths.ToUnixPath(path));
                }
            }

            Program.Instance.SaveConfiguration();
        }

        private void ReloadLibrarySettings()
        {
            // Attempt to reload the configuration files.
            // Don't continue to apply changes dialog if load fails.
            if (!Program.Instance.LoadConfiguration()) return;

            // Reload options.
            LibraryOptionsPage options;
            _optionsPage = options = new LibraryOptionsPage();
            _optionsPage.Load();

            // Load scan paths list.
            libraryGrid.Rows.Clear();
            foreach (var path in options.ScanPaths) libraryGrid.Rows.Add(GetScanPathRow(path));

            // Deselect
            libraryGrid.CurrentCell = null;
            if (libraryGrid.Rows.Count <= 0) return;
            libraryGrid.Rows[0].Selected = false;

            // Sort
            libraryGrid.Sort(libraryGrid.Columns[0], ListSortDirection.Ascending);
        }

        private void DisableLibraryControls()
        {
            // Context menu.
            libraryContextMenu.Enabled = false;

            // Buttons
            librarySaveButton.Enabled = false;
            libraryCancelButton.Enabled = false;
            tracklistButton.Enabled = false;
            deleteIndexButton.Enabled = false;
            rebuildIndexButton.Enabled = false;

            Application.DoEvents();
        }

        private void UpdateLibraryControls()
        {
            if (_optionsPage == null || !(_optionsPage is LibraryOptionsPage)) return;
            var options = (LibraryOptionsPage) _optionsPage;

            // Save and cancel buttons.
            librarySaveButton.Enabled = options.Changed;
            libraryCancelButton.Enabled = options.Changed;

            // Context menu.
            var indexing = Program.Instance.Indexing;
            libraryContextMenu.Enabled = !indexing;

            // Re-index, tracklist and delete index buttons.
            var savedPaths = options.SavedScanPathCount;
            rebuildIndexButton.Enabled = !indexing && savedPaths > 0;
            tracklistButton.Enabled = !indexing && savedPaths > 0;
            deleteIndexButton.Enabled = !indexing && savedPaths > 0;
        }

        private static DataGridViewRow GetScanPathRow(string path)
        {
            var row = new DataGridViewRow();
            var str = WindarPaths.ToWindowsPath(path);
            row.Cells.Add(new DataGridViewTextBoxCell { Value = str });
            row.Tag = str;
            return row;
        }

        private static DataGridViewRow GetNewScanPathRow(string path)
        {
            var row = new DataGridViewRow();
            row.Cells.Add(new DataGridViewTextBoxCell { Value = path });
            return row;
        }

        private void addLibPathMenuItem_Click(object sender, EventArgs e)
        {
            AddScanPath();
        }

        private void AddScanPath()
        {
            var path = SelectScanPath();
            if (string.IsNullOrEmpty(path)) return;

            // Ensure the path isn't already in the list.
            foreach (var obj in libraryGrid.Rows)
            {
                var row = (DataGridViewRow) obj;
                var value = (string) row.Cells[0].Value;
                if (path == value)
                {
                    row.Selected = true;
                    return;
                }
            }

            libraryGrid.Rows.Add(GetNewScanPathRow(path));
            ((LibraryOptionsPage) _optionsPage).ScanPathsToAdd = true;

            UpdateLibraryControls();

            // Deselect
            libraryGrid.CurrentCell = null;
            if (libraryGrid.Rows.Count <= 0) return;
            libraryGrid.Rows[0].Selected = false;

            // Sort
            libraryGrid.Sort(libraryGrid.Columns[0], ListSortDirection.Ascending);
        }

        private void removeLibPathMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var obj in libraryGrid.SelectedRows)
            {
                var row = (DataGridViewRow) obj;
                if (row.Tag != null)
                {
                    var path = WindarPaths.ToUnixPath((string) row.Tag);
                    ((LibraryOptionsPage) _optionsPage).RemoveScanPath(path);
                }
                libraryGrid.Rows.Remove(row);
            }

            UpdateLibraryControls();

            // Deselect
            libraryGrid.CurrentCell = null;
            if (libraryGrid.Rows.Count <= 0) return;
            libraryGrid.Rows[0].Selected = false;

            // Sort
            libraryGrid.Sort(libraryGrid.Columns[0], ListSortDirection.Ascending);
        }

        private void librarySaveButton_Click(object sender, EventArgs e)
        {
            BuildLibrary(false);
        }

        private void libraryCancelButton_Click(object sender, EventArgs e)
        {
            Program.Instance.LoadConfiguration();
            InitialiseLibraryPage();

            // Deselect
            libraryGrid.CurrentCell = null;
            if (libraryGrid.Rows.Count <= 0) return;
            libraryGrid.Rows[0].Selected = false;

            // Sort
            libraryGrid.Sort(libraryGrid.Columns[0], ListSortDirection.Ascending);
        }

        private void deleteIndexButton_Click(object sender, EventArgs e)
        {
            var msg = new StringBuilder();
            msg.Append("Are you sure you want to delete the current index?").Append(Environment.NewLine);
            msg.Append("Playdar will be restarted.");
            if (!Program.ShowYesNoDialog(msg.ToString())) return;

            libraryGrid.Rows.Clear();
            DisableLibraryControls();
            Application.DoEvents();

            // Show waiting dialog while deleting index files.
            Program.Instance.WaitingDialog.Do = DeleteLibraryIndexFiles;
            Program.Instance.WaitingDialog.StatusLabel.Text = "Deleting index files...";
            Program.Instance.WaitingDialog.ShowDialog();

            // Delete each path from options.
            var options = (LibraryOptionsPage) _optionsPage;
            foreach (var path in options.ScanPaths) options.RemoveScanPath(path);

            SaveLibrarySettings();
            ReloadLibrarySettings();
            UpdateLibraryControls();
        }

        private static void DeleteLibraryIndexFiles()
        {
            Program.Instance.Daemon.Stop();
            var libraryFilename = Program.Instance.Paths.PlaydarDataPath + @"\library.db";
            var libraryIndexFilename = Program.Instance.Paths.PlaydarDataPath + @"\library_index.db";
            var libraryFileInfo = new FileInfo(libraryFilename);
            var libraryIndexFileInfo = new FileInfo(libraryIndexFilename);
            if (libraryFileInfo.Exists) libraryFileInfo.Delete();
            if (libraryIndexFileInfo.Exists) libraryIndexFileInfo.Delete();
            Program.Instance.Daemon.Start();
        }

        private void rebuildIndexButton_Click(object sender, EventArgs e)
        {
            BuildLibrary(true);
        }

        private void BuildLibrary(bool alwaysRebuild)
        {
            var options = (LibraryOptionsPage) _optionsPage;
            if (options.ScanPathsRemoved || alwaysRebuild)
            {
                var msg = new StringBuilder();
                msg.Append("Playdar will need to be restarted.").Append(Environment.NewLine);
                msg.Append("Do you wish to continue?");
                if (!Program.ShowYesNoDialog(msg.ToString())) return;

                DisableLibraryControls();

                // Show waiting dialog while deleting index files.
                Program.Instance.WaitingDialog.Do = DeleteLibraryIndexFiles;
                Program.Instance.WaitingDialog.StatusLabel.Text = "Deleting index files...";
                Program.Instance.WaitingDialog.ShowDialog();
                
                SaveLibrarySettings();

                // Queue the folders to be re-scanned.
                Program.Instance.ShowTrayInfo("Scan started.");
                foreach (var path in options.ScanPaths)
                    Program.Instance.AddScanPath(path);

                ReloadLibrarySettings();

                // Show waiting dialog while scanning.
                Program.Instance.WaitingDialog.Do = null;
                Program.Instance.WaitingDialog.StatusLabel.Text = "Scanning...";
                Program.Instance.WaitingDialog.ShowDialog();
            }
            else
            {
                DisableLibraryControls();

                // Queue the new folders to be scanned.
                Program.Instance.ShowTrayInfo("Scan started.");
                foreach (var obj in libraryGrid.Rows)
                {
                    var row = (DataGridViewRow) obj;
                    if (row.Tag != null) continue;
                    var value = (string) row.Cells[0].Value;

                    // Deselect
                    libraryGrid.CurrentCell = null;
                    if (libraryGrid.Rows.Count <= 0) return;
                    libraryGrid.Rows[0].Selected = false;

                    Program.Instance.AddScanPath(value);
                }

                // Show waiting dialog while scanning.
                Program.Instance.WaitingDialog.Do = null;
                Program.Instance.WaitingDialog.StatusLabel.Text = "Scanning...";
                Program.Instance.WaitingDialog.ShowDialog();

                SaveLibrarySettings();
                ReloadLibrarySettings();
            }
            UpdateLibraryControls();
        }

        internal void ScanCompleted()
        {
            Program.Instance.WaitingDialog.Stop();
            UpdateLibraryControls();
        }

        private void tracklistButton_Click(object sender, EventArgs e)
        {
            Program.Instance.WaitingDialog.Do = ShowTracklist;
            Program.Instance.WaitingDialog.StatusLabel.Text = "Getting tracklist...";
            Program.Instance.WaitingDialog.ShowDialog();
        }

        private static void ShowTracklist()
        {
            var lines = Program.Instance.Daemon.DumpLibrary().Split('\n');
            var build = new StringBuilder();
            foreach (var str in lines)
            {
                var line = str.Trim();
                if (line.Length == 0) continue;
                var fields = line.Split('\t');
                if (fields.Length < 6)
                {
                    foreach (var field in fields)
                        build.Append(field).Append('\t');
                    break;
                }

                // Artist
                build.Append(fields[0]);

                // Album
                //build.Append(" - ");
                //build.Append(fields[1]);

                // Track
                build.Append(" - ");
                build.Append(fields[2]);

                // Type
                //build.Append(" (");
                //build.Append(fields[3]);
                //build.Append(")");

                // Filename
                //build.Append(Environment.NewLine);
                //build.Append("<");
                //build.Append(fields[4]);
                //build.Append("> ");

                // Length
                //build.Append(fields[5]);

                build.Append('\n');
            }

            // Sort the formatted lines.
            lines = build.ToString().Trim().Split('\n');
            Array.Sort(lines);

            var n = 0;
            build = new StringBuilder();
            foreach (var line in lines)
            {
                n++;
                build.Append(line);
                build.Append(Environment.NewLine);
            }

            Program.Instance.WaitingDialog.Stop();
            var d = new ShowTracklistCallback(ShowTracklistOutput);
            var args = new object[] { build.ToString().Trim(), n };
            Program.Instance.MainForm.Invoke(d, args);
        }

        private static void ShowTracklistOutput(string text, int n)
        {
            var dialog = new OutputDisplay
            {
                TextBox = { Text = text },
                Text = n == 1 ? "Track List (1 track)" : "Track List (" + n + " tracks)"
            };

            dialog.TextBox.Font = new Font(
                dialog.TextBox.Font.FontFamily,
                dialog.TextBox.Font.Size,
                dialog.TextBox.Font.Style);

            dialog.ShowDialog();
        }

        private void libraryGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (libraryGrid.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.Cell) return;
            foreach (var obj in libraryGrid.SelectedRows) ((DataGridViewRow) obj).Selected = false;
            libraryGrid.CurrentCell = null;
        }

        private void libraryGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            libraryContextMenu.Items["removeLibPathMenuItem"].Visible = libraryGrid.SelectedRows.Count > 0;
        }

        private void libraryGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (libraryGrid.Rows.Count <= 0 || libraryGrid.Rows[e.RowIndex].Tag == null) return;
            ((LibraryOptionsPage) _optionsPage).ScanPathValueChanged = true;
            UpdateLibraryControls();
        }

        private void libraryGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            var cell = libraryGrid[e.ColumnIndex, e.RowIndex];
            if (cell.GetContentBounds(e.RowIndex).Contains(e.Location)) cellEndEditTimer.Start();
        }

        private void libraryGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            var cell = libraryGrid[e.ColumnIndex, e.RowIndex];
            if (!cell.GetContentBounds(e.RowIndex).Contains(e.Location)) return;
            var str = (string) cell.Value;
            var path = string.IsNullOrEmpty(str) ? SelectScanPath() : SelectScanPath(str);
            if (!string.IsNullOrEmpty(path)) cell.Value = path;
        }

        private string SelectScanPath()
        {
            return SelectScanPath(null);
        }

        private string SelectScanPath(string initialFolder)
        {
            string result = null;
            const string description = "Select a folder to be scanned. Successfully scanned files will be added to the Playdar content library.";
            try
            {
                var dialog = new DirectoryDialog
                {
                    BrowseFor = DirectoryDialog.BrowseForTypes.Directories,
                    Title = description
                };
                
                if (!string.IsNullOrEmpty(initialFolder)) 
                    dialog.InitialPath = initialFolder;
                
                _inDirectoryDialog = true;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    result = dialog.Selected;
                _inDirectoryDialog = false;
            }
            catch (AccessViolationException ex)
            {
                if (Log.IsErrorEnabled) Log.Error("DirectoryDialog. " + ex.Message);

                // NOTE: Exception thrown on XP, related to P/Invoke.
                // Fall back on .NET alternative.
                var dialog = new FolderBrowserDialog
                                 {
                                     Description = description,
                                     ShowNewFolderButton = false
                                 };

                _inDirectoryDialog = true;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    result = dialog.SelectedPath;
                _inDirectoryDialog = false;
            }
            return result;
        }

        private void libraryGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ((LibraryOptionsPage) _optionsPage).ScanPathValueChanged = true;
            UpdateLibraryControls();
        }

        #endregion

        #region Resolvers

        private void InitialiseModulesPage()
        {
            PlaydarModulesPage options;
            _optionsPage = options = new PlaydarModulesPage();
            _optionsPage.Load();
        }

        private void modsGrid_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void modsGrid_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void modsGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void modsGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void modsGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        #endregion

        #region Plugins

        private void InitialisePluginsPage()
        {
            PluginsPage options;
            _optionsPage = options = new PluginsPage();
            _optionsPage.Load();
        }

        private void pluginsGrid_MouseDown(object sender, MouseEventArgs e)
        {

        }

        #endregion

        #region Plugin properties.

        private void InitialisePluginsPropertiesPage()
        {
            PluginPropertiesPage options;
            _optionsPage = options = new PluginPropertiesPage();
            _optionsPage.Load();
        }

        private void propsGrid_MouseDown(object sender, MouseEventArgs e)
        {

        }

        #endregion

        #endregion
    }
}
