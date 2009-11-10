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
using System.Windows.Forms;

namespace Windar
{
    public partial class DaemonBrowser : Form
    {
        private const string Homepage = "http://localhost:60210/";

        private string _lastLink;
        private bool _reallyClose;

        public DaemonBrowser()
        {
            InitializeComponent();
        }

        private void DaemonWebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.ToString().StartsWith(Homepage)) return;
            e.Cancel = true;
            System.Diagnostics.Process.Start(e.Url.ToString());
        }

        private void DaemonWebBrowser_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            if (!_lastLink.StartsWith(Homepage))
            {
                DaemonWebBrowser.Navigate(_lastLink);
            }
            else
            {
                System.Diagnostics.Process.Start(_lastLink);
            }
        }

        private void DaemonWebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (DaemonWebBrowser.Document == null) return;
            var links = DaemonWebBrowser.Document.Links;
            foreach (HtmlElement var in links)
            {
                var.AttachEventHandler("onclick", LinkClicked);
            }
        }

        private void LinkClicked(object sender, EventArgs e)
        {
            if (DaemonWebBrowser.Document == null) return;
            var link = DaemonWebBrowser.Document.ActiveElement;
            if (link == null) return;
            _lastLink = link.GetAttribute("href");
        }

        public void GoHome()
        {
            if (!DaemonWebBrowser.Location.Equals(Homepage))
            {
                DaemonWebBrowser.Navigate(Homepage);
            }
        }

        private void DaemonBrowser_Load(object sender, EventArgs e)
        {
            Text = Text + '-' + Homepage;
        }

        internal void CloseOnExit()
        {
            _reallyClose = true;
            Close();
        }

        private void DaemonBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_reallyClose) return;
            e.Cancel = true;
            Hide();
        }
    }
}
