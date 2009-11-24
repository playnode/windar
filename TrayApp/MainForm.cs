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
using log4net.Appender;

namespace Windar.TrayApp
{
    partial class MainForm : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        private const string Homepage = "http://localhost:60210/";

        private string _lastLink;
        private bool _reallyClose;
        private bool _resizing;
        private Size _oldSize;
        private Timer _logBoxTimer;

        #region Properties

        private WebBrowser _playdarBrowser;
        private RichTextBox _logBox;
        private TabPage _logBoxTab;
        private MemoryAppender _memoryAppender;

        private WebBrowser PlaydarBrowser
        {
            get
            {
                if (_playdarBrowser == null)
                {
                    var ctrl = mainformTabControl.Controls.Find("playdarBrowser", true);
                    if (ctrl.Length <= 0) throw new ApplicationException("Didn't find playdarBrowser!");
                    _playdarBrowser = (WebBrowser) ctrl[0];
                }
                return _playdarBrowser;
            }
        }

        private RichTextBox LogBox
        {
            get
            {
                if (_logBox == null)
                {
                    var ctrl = mainformTabControl.Controls.Find("logBox", true);
                    if (ctrl.Length <= 0) throw new ApplicationException("Didn't find logBox!");
                    _logBox = (RichTextBox) ctrl[0];
                }
                return _logBox;
            }
        }

        private TabPage LogBoxTab
        {
            get
            {
                if (_logBoxTab == null)
                {
                    var ctrl = mainformTabControl.Controls.Find("logTabPage", true);
                    if (ctrl.Length <= 0) throw new ApplicationException("Didn't find logTabPage!");
                    _logBoxTab = (TabPage) ctrl[0];
                }
                return _logBoxTab;
            }
        }

        private MemoryAppender MemoryAppender
        {
            get
            {
                if (_memoryAppender == null)
                {
                    var appenders = LogManager.GetRepository().GetAppenders();
                    foreach (var appender in appenders)
                    {
                        if (appender.Name != "MemoryAppender") continue;
                        _memoryAppender = (MemoryAppender) appender;
                        break;
                    }
                }
                return _memoryAppender;
            }
        }

        #endregion

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
            GoToAbout();
            InitLogBox();

            LoadPlaydarHomepage();
        }

        #endregion

        #region Page navigation.

        public void GoToAbout()
        {
            var page = mainformTabControl.TabPages["aboutTabPage"];
            mainformTabControl.SelectTab(page);
        }

        public void GoToPlaydarDaemon()
        {
            LoadPlaydarHomepage();
            var page = mainformTabControl.TabPages["playdarTabPage"];
            mainformTabControl.SelectTab(page);
        }

        #endregion

        #region Playdar daemon browser.

        private void LoadPlaydarHomepage()
        {
            if (PlaydarBrowser.Document == null
                || PlaydarBrowser.Document.Url == null
                || !PlaydarBrowser.Document.Url.Equals(Homepage))
            {
                PlaydarBrowser.Navigate(Homepage);
            }
        }

        private void playdarLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.playdar.org/");
        }

        private void playdarBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.ToString().StartsWith(Homepage)) return;
            if (e.Url.ToString().StartsWith("res://ieframe.dll/navcancl.htm"))
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = true;
                System.Diagnostics.Process.Start(e.Url.ToString());
            }
        }

        private void playdarBrowser_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            if (!_lastLink.StartsWith(Homepage))
            {
                PlaydarBrowser.Navigate(_lastLink);
            }
            else
            {
                System.Diagnostics.Process.Start(_lastLink);
            }
        }

        private void LinkClicked(object sender, EventArgs e)
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
                var.AttachEventHandler("onclick", LinkClicked);
            }
        }

        #endregion

        #region Closing

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_reallyClose) return;
            e.Cancel = true;
            Hide();
            Properties.Settings.Default.MainFormVisible = false;
            Properties.Settings.Default.Save();
        }

        internal void Exit()
        {
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
            EnsureVisible();
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
            _oldSize = Size;
            _resizing = true;
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            if (mainformTabControl.SelectedTab == LogBoxTab
                && Size != _oldSize)
            {
                LogBoxScrollToEnd();
            }
            _resizing = false;
            PersistWindowLayout();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (_resizing) return;
            if (mainformTabControl.SelectedTab == LogBoxTab)
            {
                LogBoxScrollToEnd();
            }
            PersistWindowLayout();
        }

        #endregion

        public void EnsureVisible()
        {
            if (!Program.Instance.MainForm.Visible) Program.Instance.MainForm.Show();
            Program.Instance.MainForm.Activate();
            if (Properties.Settings.Default.MainFormVisible) return;
            Properties.Settings.Default.MainFormVisible = true;
            Properties.Settings.Default.Save();
        }

        #region Memory logger.

        //TODO: Update the log box periodically, limiting the contents too.
        //TODO: Provide option to follow tail (or follow tail when scrollbar near end?)

        private void InitLogBox()
        {
            // Set the log box font.
            const string preferredFontName = "ProFontWindows";
            var testFont = new Font(preferredFontName, 12, FontStyle.Regular, GraphicsUnit.Pixel);
            LogBox.Font = testFont.Name == preferredFontName ? testFont : new Font("Courier New", 13, FontStyle.Regular, GraphicsUnit.Pixel);

            // Create timer for updating.
            _logBoxTimer = new Timer {Interval = 10};
            _logBoxTimer.Tick += LogBoxTimer_Tick;
        }

        private void LogBoxTimer_Tick(object sender, EventArgs e)
        {
            UpdateLogBox();
        }

        private void UpdateLogBox()
        {
            var events = MemoryAppender.GetEvents();
            if (events.Length <= 0) return;
            var sb = new StringBuilder();
            sb.Append(LogBox.Text);
            foreach (var logEvent in events)
            {
                var msg = logEvent.RenderedMessage;
                if (msg.StartsWith("CMD.INF: "))
                {
                    sb.Append(msg.Substring(9, msg.Length - 9));
                    sb.Append('\n');
                }
                else if (msg.StartsWith("CMD.ERR: "))
                {
                    sb.Append("ERROR! ").Append(msg.Substring(9, msg.Length - 9));
                    sb.Append('\n');
                }
            }
            MemoryAppender.Clear();
            LogBox.Text = sb.ToString();
            LogBoxScrollToEnd();
        }

        private void LogBoxScrollToEnd()
        {
            LogBox.SelectionStart = LogBox.TextLength;
            LogBox.ScrollToCaret();
        }

        #endregion

        private void mainformTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Only run the log box updating timer when log is selected.
            if (mainformTabControl.SelectedTab == LogBoxTab)
            {
                _logBoxTimer.Start();
                LogBoxScrollToEnd();
            }
            else
            {
                _logBoxTimer.Stop();
            }
        }
    }
}
