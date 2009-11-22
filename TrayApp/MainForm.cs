/*
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
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace Windar.TrayApp
{
    partial class MainForm : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        private const string Homepage = "http://localhost:60210/";

        private string _lastLink;
        private bool _reallyClose;

        #region PlaydarBrowser property

        private WebBrowser _playdarBrowser;
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

        #endregion

        #region Init

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //TODO: Restore size and position.

            GoToAbout();
            LoadPlaydarHomepage();

            // Version info.
            var info = new StringBuilder();
            info.Append(Program.AssemblyProduct).Append(' ').Append(Program.AssemblyVersion);
            versionLabel.Text = info.ToString();
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
        }

        internal void Exit()
        {
            _reallyClose = true;
            Close();
        }

        #endregion

        public void EnsureVisible()
        {
            if (!Program.Instance.MainForm.Visible) Program.Instance.MainForm.Show();
            Program.Instance.MainForm.Activate();
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            if (Log.IsDebugEnabled) Log.Debug("MainForm_ResizeEnd");
            //TODO: Persist location.
            //TODO: Persist size.
        }
    }
}
