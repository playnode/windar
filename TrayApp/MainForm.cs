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
using Windar.Common;
using Windar.TrayApp.Configuration;

namespace Windar.TrayApp
{
    partial class MainForm : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern long DeleteUrlCacheEntry(string lpszUrlName);

        private bool _exiting;
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
                    if (_optionsPage is GeneralOptionsPage)
                    {
                        SaveGeneralOptions();
                    }
                }
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

        private void cellEndEditTimer_Tick(object sender, EventArgs e)
        {
            if (_optionsPage is GeneralOptionsPage) peersGrid.EndEdit();
            else if (_optionsPage is LibraryOptionsPage) libraryGrid.EndEdit();
            else if (_optionsPage is PlaydarModulesPage) modsGrid.EndEdit();
            else if (_optionsPage is PluginsPage) pluginsGrid.EndEdit();
            else if (_optionsPage is PluginPropertiesPage) propsGrid.EndEdit();
            cellEndEditTimer.Stop();
        }

        #region General options page.

        private void SetupGeneralOptionsPage()
        {
            GeneralOptionsPage options;
            _optionsPage = options = new GeneralOptionsPage();
            _optionsPage.Load();

            nodeNameTextBox.Text = options.NodeName;
            portTextBox.Text = options.Port.ToString();
            allowIncomingCheckBox.Checked = options.AllowIncoming;
            forwardCheckBox.Checked = options.ForwardQueries;
            //TODO: Autostart

            // Load peers table.
            peersGrid.Rows.Clear();
            foreach (var peer in options.Peers) peersGrid.Rows.Add(GetPeerListRow(peer));
            peersGrid.CurrentCell = null;
            if (peersGrid.Rows.Count <= 0) return;
            peersGrid.Rows[0].Selected = false;
        }

        private void UpdateGeneralOptionsButtons()
        {
            generalOptionsSaveButton.Enabled = _optionsPage.Changed;
            generalOptionsCancelButton.Enabled = _optionsPage.Changed;
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

            SetupGeneralOptionsPage();
            UpdateGeneralOptionsButtons();
            ShowApplyChangesDialog();
        }

        private void generalOptionsSaveButton_Click(object sender, EventArgs e)
        {
            SaveGeneralOptions();
        }

        private void generalOptionsCancelButton_Click(object sender, EventArgs e)
        {
            SetupGeneralOptionsPage();
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

        #region Library page.

        private void SetupLibraryPage()
        {
            LibraryOptionsPage options;
            _optionsPage = options = new LibraryOptionsPage();
            _optionsPage.Load();

            // Load scan paths list.
            libraryGrid.Rows.Clear();
            foreach (var path in options.ScanPaths) libraryGrid.Rows.Add(GetScanPathRow(path));
            libraryGrid.CurrentCell = null;
            if (libraryGrid.Rows.Count <= 0) return;
            libraryGrid.Rows[0].Selected = false;
        }

        private void UpdateLibraryPageButtons()
        {
            librarySaveButton.Enabled = _optionsPage.Changed;
            libraryCancelButton.Enabled = _optionsPage.Changed;
        }

        private void SaveLibrarySettings()
        {
            var options = (LibraryOptionsPage) _optionsPage;
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

            // Attempt to reload the configuration files.
            // Don't continue to apply changes dialog if load fails.
            if (!Program.Instance.LoadConfiguration()) return;

            SetupLibraryPage();
            UpdateLibraryPageButtons();

            //TODO: ShowRebuildIndexDialog();
        }

        private void SelectScanPath(DataGridViewCell cell)
        {
            var dialog = new DirectoryDialog
            {
                BrowseFor = DirectoryDialog.BrowseForTypes.Directories,
                Title = "Select a folder to be scanned. Successfully scanned files will be added to the Playdar content library."
            };
            if (((string) cell.Value).Length > 0) dialog.InitialPath = (string) cell.Value;
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            cell.Value = dialog.Selected;
        }

        private static DataGridViewRow GetScanPathRow(string path)
        {
            var row = new DataGridViewRow();
            var str = WindarPaths.ToWindowsPath(path);
            row.Cells.Add(new DataGridViewTextBoxCell { Value = str });
            row.Tag = str;
            return row;
        }

        private static DataGridViewRow GetScanPathRow()
        {
            var row = new DataGridViewRow();
            row.Cells.Add(new DataGridViewTextBoxCell { Value = "" });
            return row;
        }

        private static void ShowTrackListDialog()
        {
            var build = new StringBuilder();
            var lines = Program.Instance.Daemon.DumpLibrary().Split('\n');
            var n = 0;
            foreach (var str in lines)
            {
                var line = str.Trim();
                if (line.Length == 0) break;
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

                build.Append(Environment.NewLine);
                n++;
            }

            var dialog = new OutputDisplay
            {
                TextBox = { Text = build.ToString().Trim() },
                Text = n == 1 ? "Track List (1 track)" : "Track List (" + n + " tracks)"
            };

            dialog.TextBox.Font = new Font(
                dialog.TextBox.Font.FontFamily,
                dialog.TextBox.Font.Size,
                dialog.TextBox.Font.Style);

            dialog.ShowDialog();
        }

        private void addLibPathMenuItem_Click(object sender, EventArgs e)
        {
            libraryGrid.Rows.Add(GetScanPathRow());
            ((LibraryOptionsPage) _optionsPage).NewPathsToAdd = true;
            UpdateLibraryPageButtons();

            // Select the first cell the new row.
            var row = libraryGrid.Rows[libraryGrid.Rows.Count - 1];
            libraryGrid.CurrentCell = row.Cells[0];
            libraryGrid.BeginEdit(false);
        }

        private void removeLibPathMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var obj in libraryGrid.SelectedRows)
            {
                var row = (DataGridViewRow) obj;
                if (row.Tag != null)
                {
                    var path = (string) row.Tag;
                    ((LibraryOptionsPage) _optionsPage).RemoveScanPath(path);
                }
                libraryGrid.Rows.Remove(row);
            }
            UpdateLibraryPageButtons();
        }

        private void librarySaveButton_Click(object sender, EventArgs e)
        {
            SaveLibrarySettings();
        }

        private void libraryCancelButton_Click(object sender, EventArgs e)
        {
            SetupLibraryPage();
            librarySaveButton.Enabled = _optionsPage.Changed;
            libraryCancelButton.Enabled = _optionsPage.Changed;
        }

        private void reindexButton_Click(object sender, EventArgs e)
        {
            //TODO
        }

        private void tracklistButton_Click(object sender, EventArgs e)
        {
            ShowTrackListDialog();
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
            UpdateLibraryPageButtons();
        }

        private void libraryGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var cell = libraryGrid[e.ColumnIndex, e.RowIndex];
            if (cell.GetContentBounds(e.RowIndex).Contains(e.Location)) cellEndEditTimer.Start();
        }

        private void libraryGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var cell = libraryGrid[e.ColumnIndex, e.RowIndex];
            if (cell.GetContentBounds(e.RowIndex).Contains(e.Location)) SelectScanPath(cell);
        }

        private void libraryGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ((LibraryOptionsPage) _optionsPage).ScanPathValueChanged = true;
            UpdateLibraryPageButtons();
        }

        #endregion

        #region Playdar modules

        private void SetupModulesPage()
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

        private void pluginsGrid_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void propsGrid_MouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}
