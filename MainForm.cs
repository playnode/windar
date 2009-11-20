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
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Windar
{
    public partial class MainForm : Form
    {
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

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GoToAbout();
            LoadPlaydarHomepage();

            // Version info.
            var info = new StringBuilder();
            info.Append(AssemblyProduct).Append(' ').Append(AssemblyVersion);
            versionLabel.Text = info.ToString();
        }

        public void EnsureVisible()
        {
            if (!Program.Instance.MainForm.Visible) Program.Instance.MainForm.Show();
            else
            {
                Program.Instance.MainForm.TopMost = !Program.Instance.MainForm.TopMost;
                Program.Instance.MainForm.TopMost = !Program.Instance.MainForm.TopMost;
                Program.Instance.MainForm.BringToFront();
                Program.Instance.MainForm.Focus();
            }
        }

        public void GoToAbout()
        {
            var page = mainformTabControl.TabPages["aboutTabPage"];
            mainformTabControl.SelectTab(page);
        }

        private void LoadPlaydarHomepage()
        {
            if (PlaydarBrowser.Document == null
                || PlaydarBrowser.Document.Url == null
                || !PlaydarBrowser.Document.Url.Equals(Homepage))
            {
                PlaydarBrowser.Navigate(Homepage);
            }
        }

        public void GoToPlaydarDaemon()
        {
            LoadPlaydarHomepage();
            var page = mainformTabControl.TabPages["playdarTabPage"];
            mainformTabControl.SelectTab(page);
        }

        internal void CloseOnExit()
        {
            _reallyClose = true;
            Close();
        }

        private void playdarLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.playdar.org/");
        }

        #region Assembly Attribute Accessors

        private static string AssemblyTitle
        {
            get
            {
                // Get all Title attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    var titleAttribute = (AssemblyTitleAttribute)attributes[0];

                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "") return titleAttribute.Title;
                }

                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        private static string AssemblyVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        private static string AssemblyDescription
        {
            get
            {
                // Get all Description attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

                // If there aren't any Description attributes, return an empty string
                // If there is a Description attribute, return its value
                return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        private static string AssemblyProduct
        {
            get
            {
                // Get all Product attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);

                // If there aren't any Product attributes, return an empty string
                // If there is a Product attribute, return its value
                return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        private static string AssemblyCopyright
        {
            get
            {
                // Get all Copyright attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

                // If there aren't any Copyright attributes, return an empty string
                // If there is a Copyright attribute, return its value
                return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        private static string AssemblyCompany
        {
            get
            {
                // Get all Company attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

                // If there aren't any Company attributes, return an empty string
                // If there is a Company attribute, return its value
                return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        private static string AssemblyTrademark
        {
            get
            {
                // Get all Trademark attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTrademarkAttribute), false);

                // If there aren't any Trademark attributes, return an empty string
                // If there is a Trademark attribute, return its value
                return attributes.Length == 0 ? "" : ((AssemblyTrademarkAttribute)attributes[0]).Trademark;
            }
        }

        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_reallyClose) return;
            e.Cancel = true;
            Hide();
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
    }
}
