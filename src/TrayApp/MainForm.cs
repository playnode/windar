﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using log4net;
using Newtonsoft.Json.Linq;
using Windar.Common;
using Windar.TrayApp.Configuration;

namespace Windar.TrayApp
{
    partial class MainForm : Form
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        internal delegate void ShowTracklistCallback(string text, int n);
        internal delegate void PlaydarStateChangedCallback(bool available);

        [DllImport("wininet.dll", SetLastError = true)]
        static extern long DeleteUrlCacheEntry(string lpszUrlName);

        bool _exiting;
        bool _resizing;
        bool _inDirectoryDialog;
        string _lastLink;
        TabPage _lastSelectedTab;
        FormWindowState _lastWindowState;
        Size _oldSize;
        IOptionsPage _optionsPage;
        int _navLoopCount;
        BackgroundWorker _backgroundPoller;
        bool _playdarLastPolledRunning = true;

        #region Init

        public MainForm()
        {
            InitializeComponent();
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            // Version info for the About page.
            StringBuilder info = new StringBuilder();
            info.Append(Program.AssemblyProduct).Append(' ').Append(Program.AssemblyVersion);
            versionLabel.Text = info.ToString();

            RestoreWindowLayout();
            GoToAboutPage();

#if DEBUG
            logBox.Load();
            logBox.VScroll += RichTextBoxPlus_VScroll;
            logBox.ScrollToEnd();
#else
            mainTabControl.TabPages.Remove(logTabPage);
            logTabPage = null;
#endif

            //TODO: Re-add tabs.
            optionsTabControl.TabPages.Remove(modsTabPage);
            optionsTabControl.TabPages.Remove(pluginsTabPage);
            optionsTabControl.TabPages.Remove(propsTabPage);

            // Start the Playdar polling timer.
            playdarPoller.Start();

            ShowDaemonPage();
            LoadPlaydarHomepage();
        }

        void RichTextBoxPlus_VScroll(object sender, EventArgs e)
        {
            if (logBox != null)
            {
                Program.Instance.MainForm.followTailCheckBox.Checked = logBox.FollowTail;
            }
        }

        #endregion

        public void EnsureVisible()
        {
            if (!Visible)
            {
                // Gracefull restore the window.
                WindowState = FormWindowState.Minimized;
                Show();

                // Restore window state.
                WindowState = _lastWindowState;

                // Attempt to bring the window to front.
                Program.Instance.MainForm.Activate();

                // Persist the visibility state.
                if (Properties.Settings.Default.MainFormVisible) return;
                Properties.Settings.Default.MainFormVisible = true;
                Properties.Settings.Default.Save();
            }
        }

        public bool InModalDialog()
        {
            bool modal = _inDirectoryDialog;
            if (modal) EnsureVisible();
            return modal;
        }

        #region Navigation methods called from outside MainForm.

        public void GoToAboutPage()
        {
            if (InModalDialog()) return;
            if (mainTabControl.SelectedTab != aboutTabPage)
            {
                if (!Program.Instance.MainForm.Visible)
                {
                    Program.Instance.MainForm.Opacity = 0;
                    mainTabControl.SelectTab(aboutTabPage);
                    Program.Instance.MainForm.Opacity = 1;
                }
                else
                {
                    mainTabControl.SelectTab(aboutTabPage);
                }
            }
            Program.Instance.MainForm.EnsureVisible();
        }

        public void GoToPlaydarHomePage()
        {
            if (InModalDialog()) return;
            if (mainTabControl.SelectedTab != playdarTabPage)
            {
                if (!Program.Instance.MainForm.Visible)
                {
                    Program.Instance.MainForm.Opacity = 0;
                    mainTabControl.SelectTab(playdarTabPage);
                    Program.Instance.MainForm.Opacity = 1;
                }
                else
                {
                    mainTabControl.SelectTab(playdarTabPage);
                }
            }
            Program.Instance.MainForm.EnsureVisible();
            LoadPlaydarHomepage();
        }

        public void GoToAddScanFolder()
        {
            if (InModalDialog()) return;
            if (mainTabControl.SelectedTab != optionsTabPage)
            {
                if (!Program.Instance.MainForm.Visible)
                {
                    Program.Instance.MainForm.Opacity = 0;
                    mainTabControl.SelectTab(optionsTabPage);
                    Program.Instance.MainForm.Opacity = 1;
                }
                else
                {
                    mainTabControl.SelectTab(optionsTabPage);
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
            if (playdarBrowser.Document == null
                || playdarBrowser.Document.Url == null
                || !playdarBrowser.Document.Url.Equals(Program.Instance.PlaydarDaemon))
            {
                playdarBrowser.Navigate(Program.Instance.PlaydarDaemon);
            }
        }

        internal void ShowDaemonPage()
        {
            ShowDaemonPage(null, false);
        }

        internal void ShowDaemonPage(string text, bool newPage)
        {
            if (string.IsNullOrEmpty(text)) text = "&nbsp;";
            playdarBrowser.Stop();
            StringBuilder html = new StringBuilder();
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
            if (playdarBrowser.Document != null) 
            {
                playdarBrowser.Document.OpenNew(!newPage);
                playdarBrowser.Document.Write(html.ToString());
            }
            else
            {
                playdarBrowser.DocumentText = html.ToString();
            }
        }

        void startDaemonButton_Click(object sender, EventArgs e)
        {
            Program.Instance.StartDaemon();
        }

        void stopDaemonButton_Click(object sender, EventArgs e)
        {
            Program.Instance.StopDaemon();
        }

        void restartButton_Click(object sender, EventArgs e)
        {
            Program.Instance.RestartDaemon();
        }

        void homeButton_Click(object sender, EventArgs e)
        {
            if (Program.Instance.Daemon.Started)
            {
                LoadPlaydarHomepage();
            }
        }

        void backButton_Click(object sender, EventArgs e)
        {
            if (Program.Instance.Daemon.Started)
            {
                playdarBrowser.GoBack();
            }
        }

        void refreshButton_Click(object sender, EventArgs e)
        {
            if (Program.Instance.Daemon.Started) playdarBrowser.Refresh();
        }

        void playdarLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.playdar.org/");
        }

        void playdarBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            string url = e.Url.ToString();
            if (Log.IsDebugEnabled) Log.Debug("Navigating to " + url);

            // Protect from loop if Internet Explorer is going haywire.
            if (!url.Equals("about:blank")) _navLoopCount = 0;
            else if (_navLoopCount++ > 2) return;

            // Make sure the page isn't from the cache!
            DeleteUrlCacheEntry(url);

            // Handle some expected pages.
            if (url.Equals(Program.Instance.PlaydarDaemon))
            {
                homeButton.Enabled = false;
                backButton.Enabled = false;
                return;
            }
            if (url.StartsWith(Program.Instance.PlaydarDaemon))
            {
                homeButton.Enabled = true;
                backButton.Enabled = true;
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

        void playdarBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            if (!_lastLink.StartsWith(Program.Instance.PlaydarDaemon))
            {
                playdarBrowser.Navigate(_lastLink);
            }
            else
            {
                System.Diagnostics.Process.Start(_lastLink);
            }
        }

        void PlaydarBrowserLinkClicked(object sender, EventArgs e)
        {
            if (playdarBrowser.Document == null) return;
            HtmlElement link = playdarBrowser.Document.ActiveElement;
            if (link == null) return;
            _lastLink = link.GetAttribute("href");
        }

        void playdarBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string url = e.Url.ToString();

            if (Log.IsDebugEnabled)
            {
                StringBuilder msg = new StringBuilder();
                msg.Append("Document completed. URL = \"").Append(url).Append('"');
                if (playdarBrowser.Document != null)
                {
                    msg.Append(", Title = ");
                    msg.Append(playdarBrowser.Document.Title);
                }
                Log.Debug(msg.ToString());
            }

            refreshButton.Enabled = true;
            if (url.Equals(Program.Instance.PlaydarDaemon))
            {
                backButton.Enabled = homeButton.Enabled = false;
            }
            else
            {
                backButton.Enabled = playdarBrowser.CanGoBack;
                homeButton.Enabled = true;
            }

            if (playdarBrowser.Document == null) return;

            if (playdarBrowser.Document.Title == "Internet Explorer cannot display the webpage")
            {
                refreshButton.Enabled = false;
                if (url.Equals(Program.Instance.PlaydarDaemon))
                {
                    //startDaemonButton.Enabled = true;
                    //restartDaemonButton.Enabled = false;
                    //stopDaemonButton.Enabled = false;
                    ShowDaemonPage("Playdar service not running.", false);
                }
                else
                {
                    ShowDaemonPage("Page not available.", false);
                }
                return;
            }

            // Attach click event handler to all javascript links too.
            HtmlElementCollection links = playdarBrowser.Document.Links;
            foreach (HtmlElement var in links)
            {
                var.AttachEventHandler("onclick", PlaydarBrowserLinkClicked);
            }
        }

        #endregion

        void followTailCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (logBox == null) return;
            logBox.FollowTail = followTailCheckBox.Checked;
            if (logBox.FollowTail) logBox.ScrollToEnd();
        }

        #region Closing

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mainTabControl.SelectedTab == optionsTabPage
                && Program.Instance.Config != null
                && (e.Cancel = !ApplyOptionsOrCancel())) return;

            if (_exiting) return;

            if (!Program.Instance.Daemon.Started)
            {
                Program.Shutdown();
            }
            else
            {
                // Hide instead of close.
                e.Cancel = true;
                Hide();
                
                // Always return to the about page.
                mainTabControl.SelectTab(aboutTabPage);
                
                // Persist the visibility state.
                if (!Properties.Settings.Default.MainFormVisible) return;
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

        void PersistWindowLayout()
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

        void RestoreWindowLayout()
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

        void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            _resizing = true;
            _oldSize = Size;
        }

        void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            if (mainTabControl.SelectedTab == logTabPage
                && logTabPage != null
                && Size != _oldSize
                && logBox != null) logBox.ScrollToEnd();
            _resizing = false;
            PersistWindowLayout();
        }

        void MainForm_Resize(object sender, EventArgs e)
        {
            if (_resizing) return;
            if (mainTabControl.SelectedTab == logTabPage
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

        void MainTabControl_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (_lastSelectedTab != optionsTabPage) return;
            if (Program.Instance.Config == null) e.Cancel = true;
            else e.Cancel = !ApplyOptionsOrCancel();
        }

        void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Remember last selected tab.
            _lastSelectedTab = mainTabControl.SelectedTab;

            // Check the new selected tab.
            if (mainTabControl.SelectedTab == logTabPage
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
                if (mainTabControl.SelectedTab == playdarTabPage)
                {
                    GoToPlaydarHomePage();
                }
                if (mainTabControl.SelectedTab == optionsTabPage)
                {
                    // Attempt to reload the configuration here.
                    // If it fails the program will be shutdown.
                    if (!Program.Instance.LoadConfiguration()) return;

                    optionsTabControl.SelectTab(libraryTabPage);
                    InitialiseLibraryPage();
                }
            }
        }

        void optionsTabControl_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            e.Cancel = !ApplyOptionsOrCancel();
        }

        void optionsTabControl_SelectedIndexChanged(object sender, EventArgs e)
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
        }

        #endregion

        #region Settings pages.

        /// <summary>
        /// Check if changes. Require save or cancel changes.
        /// </summary>
        /// <returns>Returns true if ok to proceed, false otherwise.</returns>
        bool ApplyOptionsOrCancel()
        {
            if (Visible && _optionsPage != null && _optionsPage.Changed)
            {
                DialogResult result = Program.ShowYesNoCancelDialog("Save changes?");
                if (result == DialogResult.Cancel) return false;
                if (result == DialogResult.No) _optionsPage = null;
                else if (_optionsPage is GeneralOptionsPage) SaveGeneralOptions();
                else if (_optionsPage is LibraryOptionsPage) BuildLibrary(false);
                ResetOptionsPagesButtons();
            }
            return true;
        }

        void ResetOptionsPagesButtons()
        {
            UpdateGeneralOptionsButtons();
            UpdateLibraryControls();
        }

        void cellEndEditTimer_Tick(object sender, EventArgs e)
        {
            if (_optionsPage is GeneralOptionsPage) peersGrid.EndEdit();
            else if (_optionsPage is LibraryOptionsPage) libraryGrid.EndEdit();
            else if (_optionsPage is PlaydarModulesPage) modsGrid.EndEdit();
            else if (_optionsPage is PluginsPage) pluginsGrid.EndEdit();
            else if (_optionsPage is PluginPropertiesPage) propsGrid.EndEdit();
            cellEndEditTimer.Stop();
        }

        #region General options.

        void InitialiseGeneralOptionsPage()
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
            foreach (PeerInfo peer in options.Peers) peersGrid.Rows.Add(GetPeerListRow(peer));
            peersGrid.CurrentCell = null;
            if (peersGrid.Rows.Count <= 0) return;
            peersGrid.Rows[0].Selected = false;
        }

        void UpdateGeneralOptionsButtons()
        {
            bool state = _optionsPage != null && _optionsPage is GeneralOptionsPage;
            state = state ? _optionsPage.Changed : false;
            generalOptionsSaveButton.Enabled = state;
            generalOptionsCancelButton.Enabled = state;
        }

        void SaveGeneralOptions()
        {
            foreach (object obj in peersGrid.Rows)
            {
                DataGridViewRow row = (DataGridViewRow)obj;

                // Get host, port and share from row.
                string host = (string) row.Cells[0].Value;
                int port = 0;
                object portObj = row.Cells[1].Value;
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
                bool share = (bool) row.Cells[2].Value;

                // Handle new and updated rows.
                if (row.Tag != null)
                {
                    // Updating
                    PeerInfo tag = (PeerInfo)row.Tag;
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
            Program.ShowApplyChangesDialog();
        }

        void generalOptionsSaveButton_Click(object sender, EventArgs e)
        {
            SaveGeneralOptions();
        }

        void generalOptionsCancelButton_Click(object sender, EventArgs e)
        {
            Program.Instance.LoadConfiguration();
            InitialiseGeneralOptionsPage();
            generalOptionsSaveButton.Enabled = _optionsPage.Changed;
            generalOptionsCancelButton.Enabled = _optionsPage.Changed;
        }

        #region Change handling.

        void allowIncomingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ((GeneralOptionsPage) _optionsPage).AllowIncoming = allowIncomingCheckBox.Checked;
            UpdateGeneralOptionsButtons();
        }

        void autostartCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ((GeneralOptionsPage) _optionsPage).AutoStart = autostartCheckBox.Checked;
            UpdateGeneralOptionsButtons();
        }

        void forwardCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ((GeneralOptionsPage) _optionsPage).ForwardQueries = forwardCheckBox.Checked;
            UpdateGeneralOptionsButtons();
        }

        void nodeNameTextBox_TextChanged(object sender, EventArgs e)
        {
            ((GeneralOptionsPage) _optionsPage).NodeName = nodeNameTextBox.Text;
            UpdateGeneralOptionsButtons();
        }

        void portTextBox_TextChanged(object sender, EventArgs e)
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

        static DataGridViewRow GetPeerListRow(PeerInfo peer)
        {
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell cellHost = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell cellPort = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell cellShare = new DataGridViewTextBoxCell();
            cellHost.Value = peer.Host;
            cellPort.Value = peer.Port;
            cellShare.Value = peer.Share;
            row.Cells.Add(cellHost);
            row.Cells.Add(cellPort);
            row.Cells.Add(cellShare);
            row.Tag = peer;
            return row;
        }

        static DataGridViewRow GetPeerListRow()
        {
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell cellHost = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell cellPort = new DataGridViewTextBoxCell();
            DataGridViewTextBoxCell cellShare = new DataGridViewTextBoxCell();
            cellHost.Value = "";
            cellPort.Value = "60211";
            cellShare.Value = false;
            row.Cells.Add(cellHost);
            row.Cells.Add(cellPort);
            row.Cells.Add(cellShare);
            return row;
        }

        void addPeerMenuItem_Click(object sender, EventArgs e)
        {
            peersGrid.Rows.Add(GetPeerListRow());
            ((GeneralOptionsPage) _optionsPage).NewPeersToAdd = true;
            UpdateGeneralOptionsButtons();

            // Select the first cell the new row.
            DataGridViewRow row = peersGrid.Rows[peersGrid.Rows.Count - 1];
            peersGrid.CurrentCell = row.Cells[0];
            peersGrid.BeginEdit(false);
        }

        void removePeerMenuItem_Click(object sender, EventArgs e)
        {
            foreach (object obj in peersGrid.SelectedRows)
            {
                DataGridViewRow row = (DataGridViewRow)obj;
                if (row.Tag != null)
                {
                    PeerInfo peer = (PeerInfo)row.Tag;
                    ((GeneralOptionsPage) _optionsPage).RemovePeer(peer.Host, peer.Port);
                }
                peersGrid.Rows.Remove(row);
            }
            UpdateGeneralOptionsButtons();
        }

        void peersGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (peersGrid.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.Cell) return;
            foreach (object obj in peersGrid.SelectedRows) ((DataGridViewRow) obj).Selected = false;
            peersGrid.CurrentCell = null;
        }

        void peersGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            peersContextMenu.Items["removePeerMenuItem"].Visible = peersGrid.SelectedRows.Count > 0;
        }

        void peersGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (peersGrid.Rows.Count <= 0 || peersGrid.Rows[e.RowIndex].Tag == null) return;
            ((GeneralOptionsPage) _optionsPage).PeerValueChanged = true;
            UpdateGeneralOptionsButtons();
        }

        void peersGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell cell = peersGrid[e.ColumnIndex, e.RowIndex];
            if (cell.GetContentBounds(e.RowIndex).Contains(e.Location)) cellEndEditTimer.Start();
        }

        void peersGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ((GeneralOptionsPage) _optionsPage).PeerValueChanged = true;
            UpdateGeneralOptionsButtons();
        }

        #endregion

        #endregion

        #region Local library.

        void InitialiseLibraryPage()
        {
            LibraryOptionsPage options;
            _optionsPage = options = new LibraryOptionsPage();
            _optionsPage.Load();

            // Load scan paths list.
            libraryGrid.Rows.Clear();
            foreach (string path in options.ScanPaths) libraryGrid.Rows.Add(GetScanPathRow(path));

            UpdateLibraryControls();

            // Deselect
            libraryGrid.CurrentCell = null;
            if (libraryGrid.Rows.Count <= 0) return;
            libraryGrid.Rows[0].Selected = false;

            // Sort
            libraryGrid.Sort(libraryGrid.Columns[0], ListSortDirection.Ascending);
        }

        void SaveLibrarySettings()
        {
            LibraryOptionsPage options = (LibraryOptionsPage)_optionsPage;

            // Update and add paths to scan, as required.
            foreach (object obj in libraryGrid.Rows)
            {
                DataGridViewRow row = (DataGridViewRow)obj;
                string path = (string) row.Cells[0].Value;

                // Handle new and updated rows.
                if (row.Tag != null)
                {
                    // Updating
                    string tag = (string) row.Tag;
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

        void ReloadLibrarySettings()
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
            foreach (string path in options.ScanPaths) libraryGrid.Rows.Add(GetScanPathRow(path));

            // Deselect
            libraryGrid.CurrentCell = null;
            if (libraryGrid.Rows.Count <= 0) return;
            libraryGrid.Rows[0].Selected = false;

            // Sort
            libraryGrid.Sort(libraryGrid.Columns[0], ListSortDirection.Ascending);
        }

        void DisableLibraryControls()
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

        void UpdateLibraryControls()
        {
            if (_optionsPage == null || !(_optionsPage is LibraryOptionsPage)) return;
            LibraryOptionsPage options = (LibraryOptionsPage)_optionsPage;

            // Save and cancel buttons.
            librarySaveButton.Enabled = options.Changed;
            libraryCancelButton.Enabled = options.Changed;

            // Context menu.
            bool indexing = Program.Instance.Indexing;
            libraryContextMenu.Enabled = !indexing;

            // Re-index, tracklist and delete index buttons.
            int savedPaths = options.SavedScanPathCount;
            rebuildIndexButton.Enabled = !indexing && savedPaths > 0;
            tracklistButton.Enabled = !indexing && savedPaths > 0;
            deleteIndexButton.Enabled = !indexing && savedPaths > 0;
        }

        static DataGridViewRow GetScanPathRow(string path)
        {
            DataGridViewRow row = new DataGridViewRow();
            string str = WindarPaths.ToWindowsPath(path);
            DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
            cell.Value = str;
            row.Cells.Add(cell);
            row.Tag = str;
            return row;
        }

        static DataGridViewRow GetNewScanPathRow(string path)
        {
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
            cell.Value = path;
            row.Cells.Add(cell);
            return row;
        }

        void addLibPathMenuItem_Click(object sender, EventArgs e)
        {
            AddScanPath();
        }

        void AddScanPath()
        {
            string path = SelectScanPath();
            if (string.IsNullOrEmpty(path)) return;

            // Ensure the path isn't already in the list.
            foreach (object obj in libraryGrid.Rows)
            {
                DataGridViewRow row = (DataGridViewRow)obj;
                string value = (string) row.Cells[0].Value;
                if (path != value) continue;
                row.Selected = true;
                return;
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

        void removeLibPathMenuItem_Click(object sender, EventArgs e)
        {
            foreach (object obj in libraryGrid.SelectedRows)
            {
                DataGridViewRow row = (DataGridViewRow)obj;
                if (row.Tag != null)
                {
                    string path = WindarPaths.ToUnixPath((string) row.Tag);
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

        void librarySaveButton_Click(object sender, EventArgs e)
        {
            BuildLibrary(false);
        }

        void libraryCancelButton_Click(object sender, EventArgs e)
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

        void deleteIndexButton_Click(object sender, EventArgs e)
        {
            StringBuilder msg = new StringBuilder();
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
            LibraryOptionsPage options = (LibraryOptionsPage)_optionsPage;
            foreach (string path in options.ScanPaths) options.RemoveScanPath(path);

            SaveLibrarySettings();
            ReloadLibrarySettings();
            UpdateLibraryControls();
        }

        static void DeleteLibraryIndexFiles()
        {
            Program.Instance.Daemon.Stop();
            string libraryFilename = Program.Instance.Paths.WindarAppData + @"\library.db";
            string libraryIndexFilename = Program.Instance.Paths.WindarAppData + @"\library_index.db";
            FileInfo libraryFileInfo = new FileInfo(libraryFilename);
            FileInfo libraryIndexFileInfo = new FileInfo(libraryIndexFilename);
            if (libraryFileInfo.Exists) libraryFileInfo.Delete();
            if (libraryIndexFileInfo.Exists) libraryIndexFileInfo.Delete();
            Program.Instance.Daemon.Start();
        }

        void rebuildIndexButton_Click(object sender, EventArgs e)
        {
            BuildLibrary(true);
        }

        void BuildLibrary(bool alwaysRebuild)
        {
            LibraryOptionsPage options = (LibraryOptionsPage)_optionsPage;
            if (options.ScanPathsRemoved || alwaysRebuild)
            {
                StringBuilder msg = new StringBuilder();
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
                foreach (string path in options.ScanPaths)
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
                foreach (object obj in libraryGrid.Rows)
                {
                    DataGridViewRow row = (DataGridViewRow)obj;
                    if (row.Tag != null) continue;
                    string value = (string)row.Cells[0].Value;

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

        void tracklistButton_Click(object sender, EventArgs e)
        {
            Program.Instance.WaitingDialog.Do = ShowTracklist;
            Program.Instance.WaitingDialog.StatusLabel.Text = "Getting tracklist...";
            Program.Instance.WaitingDialog.ShowDialog();
        }

        static void ShowTracklist()
        {
            string[] lines = Program.Instance.Daemon.DumpLibrary().Split('\n');
            StringBuilder build = new StringBuilder();
            foreach (string str in lines)
            {
                string line = str.Trim();
                if (line.Length == 0) continue;
                string[] fields = line.Split('\t');
                if (fields.Length < 6)
                {
                    foreach (string field in fields)
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

            int n = 0;
            build = new StringBuilder();
            foreach (string line in lines)
            {
                n++;
                build.Append(line);
                build.Append(Environment.NewLine);
            }

            Program.Instance.WaitingDialog.Stop();
            ShowTracklistCallback d = new ShowTracklistCallback(ShowTracklistOutput);
            object[] args = new object[] { build.ToString().Trim(), n };
            Program.Instance.MainForm.Invoke(d, args);
        }

        static void ShowTracklistOutput(string text, int n)
        {
            OutputDisplay dialog = new OutputDisplay();
            dialog.TextBox.Text = text;
            dialog.Text = n == 1 ? "Track List (1 track)" : "Track List (" + n + " tracks)";

            dialog.TextBox.Font = new Font(
                dialog.TextBox.Font.FontFamily,
                dialog.TextBox.Font.Size,
                dialog.TextBox.Font.Style);

            dialog.ShowDialog();
        }

        void libraryGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (libraryGrid.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.Cell) return;
            foreach (object obj in libraryGrid.SelectedRows) ((DataGridViewRow) obj).Selected = false;
            libraryGrid.CurrentCell = null;
        }

        void libraryGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            libraryContextMenu.Items["removeLibPathMenuItem"].Visible = libraryGrid.SelectedRows.Count > 0;
        }

        void libraryGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (libraryGrid.Rows.Count <= 0 || libraryGrid.Rows[e.RowIndex].Tag == null) return;
            ((LibraryOptionsPage) _optionsPage).ScanPathValueChanged = true;
            UpdateLibraryControls();
        }

        void libraryGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell cell = libraryGrid[e.ColumnIndex, e.RowIndex];
            if (cell.GetContentBounds(e.RowIndex).Contains(e.Location)) cellEndEditTimer.Start();
        }

        void libraryGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell cell = libraryGrid[e.ColumnIndex, e.RowIndex];
            if (!cell.GetContentBounds(e.RowIndex).Contains(e.Location)) return;
            string str = (string) cell.Value;
            string path = string.IsNullOrEmpty(str) ? SelectScanPath() : SelectScanPath(str);
            if (!string.IsNullOrEmpty(path)) cell.Value = path;
        }

        string SelectScanPath()
        {
            return SelectScanPath(null);
        }

        string SelectScanPath(string initialFolder)
        {
            string result = null;
            DirectoryDialog dialog = new DirectoryDialog();
            dialog.BrowseFor = DirectoryDialog.BrowseForTypes.Directories;
            dialog.Title = "Select a folder to be scanned. Successfully scanned files will be added to the Playdar content library.";

            if (!string.IsNullOrEmpty(initialFolder))
                dialog.InitialPath = initialFolder;

            _inDirectoryDialog = true;
            if (dialog.ShowDialog(this) == DialogResult.OK)
                result = dialog.Selected;
            _inDirectoryDialog = false;
            return result;
        }

        void libraryGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ((LibraryOptionsPage) _optionsPage).ScanPathValueChanged = true;
            UpdateLibraryControls();
        }

        #endregion

        #region Resolvers

        void InitialiseModulesPage()
        {
            PlaydarModulesPage options;
            _optionsPage = options = new PlaydarModulesPage();
            _optionsPage.Load();
        }

        void modsGrid_MouseDown(object sender, MouseEventArgs e)
        {

        }

        void modsGrid_MouseClick(object sender, MouseEventArgs e)
        {

        }

        void modsGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        void modsGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        void modsGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        #endregion

        #region Plugins

        void InitialisePluginsPage()
        {
            PluginsPage options;
            _optionsPage = options = new PluginsPage();
            _optionsPage.Load();
        }

        void pluginsGrid_MouseDown(object sender, MouseEventArgs e)
        {

        }

        #endregion

        #region Plugin properties.

        void InitialisePluginsPropertiesPage()
        {
            PluginPropertiesPage options;
            _optionsPage = options = new PluginPropertiesPage();
            _optionsPage.Load();
        }

        void propsGrid_MouseDown(object sender, MouseEventArgs e)
        {

        }

        #endregion

        #endregion

        /// <summary>
        /// This event handler responds to the timer tick event, and is used to
        /// check whether Playdar is running using the stat method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        void playdarPoller_Tick(object sender, EventArgs e)
        {
            if (Log.IsDebugEnabled) Log.Debug("playdarPoller_Tick");

            _backgroundPoller = new BackgroundWorker();
            _backgroundPoller.DoWork += backgroundPoller_DoWork;
            _backgroundPoller.RunWorkerAsync();
        }

        /// <summary>
        /// The background worker is used to make a web request to check if
        /// Playdar is running, to avoid an issue with GUI responsiveness in
        /// case of failed web requests.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void backgroundPoller_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Log.IsDebugEnabled) Log.Debug("backgroundPoller_DoWork");

            bool playdarAvailable = false;

            // Build the request URL.
            StringBuilder url = new StringBuilder();
            url.Append(Program.Instance.Paths.LocalPlaydarUrl).Append("api/?method=stat");

            // Get and process result.
            string response = Program.WGet(url.ToString(), 100); // 0.1 secs
            if (response != null)
            {
                JObject json = JObject.Parse(response);
                playdarAvailable = DeQuotify(json["name"]).Equals("playdar");
            }

            if (Log.IsDebugEnabled) Log.Debug("Playdar available = " + playdarAvailable);

            // Check if there's a change to Playdar running state.
            if (_playdarLastPolledRunning != playdarAvailable)
            {
                PlaydarStateChangedCallback d = new PlaydarStateChangedCallback(PlaydarStateChanged);
                object[] args = new object[] { playdarAvailable };
                Program.Instance.MainForm.Invoke(d, args);
            }
        }

        internal void PlaydarStateChanged(bool available)
        {
            // Record the state change to detect next change.
            _playdarLastPolledRunning = available;

            // Set state of the daemon start/stop/restart buttons.
            startDaemonButton.Enabled = !available;
            restartDaemonButton.Enabled = available;
            stopDaemonButton.Enabled = available;
            Application.DoEvents();

            // Load the Playdar homepage if Playdar is available.
            if (!available) return;
            homeButton.Enabled = false;
            backButton.Enabled = false;
            Application.DoEvents();
            LoadPlaydarHomepage();
            Application.DoEvents();
            refreshButton.Enabled = true;
            Application.DoEvents();
        }

        static string DeQuotify(JToken token)
        {
            if (token == null) return null;
            string str = token.ToString();
            string result = str;

            if (str[0] == '"' && str[str.Length - 1] == '"')
                result = str.Substring(1, str.Length - 2);

            return result;
        }
    }
}
