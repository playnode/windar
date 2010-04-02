/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
 *
 * Windar: Playdar for Windows
 *
 * Windar is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 *
 * Windar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 ************************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Windar.Common;
using Windar.PluginAPI;

namespace Windar.MP3tunes
{
    public partial class MP3tunesConfigForm : UserControl, IConfigForm
    {
        public IConfigFormContainer FormContainer { get; set; }

        readonly MP3tunesPlugin _plugin;
        readonly SimplePropertiesFile _conf;
        string _origUsername;
        string _origPassword;
        string _partnerToken;

        public MP3tunesConfigForm(MP3tunesPlugin plugin)
        {
            InitializeComponent();
            _plugin = plugin;

            // Set the original values.
            var filename = _plugin.Host.Paths.PlaydarEtcPath + @"\mp3tunes.conf";
            _conf = new SimplePropertiesFile(filename);
            if (File.Exists(filename))
            {
                _origUsername = _conf.Sections["mp3tunes"]["username"];
                _origPassword = _conf.Sections["mp3tunes"]["password"];
                _partnerToken = !_conf.Sections["mp3tunes"].ContainsKey("partner_token")
                    ? "" : _conf.Sections["mp3tunes"]["partner_token"];
            }
            else
            {
                _origUsername = "";
                _origPassword = "";
                _partnerToken = "";
            }
        }

        void MP3tunesConfigForm_Load(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(_origUsername)) usernameTextbox.Text = _origUsername;
            if (!string.IsNullOrEmpty(_origPassword)) passwordTextbox.Text = _origPassword;
            if (string.IsNullOrEmpty(_partnerToken)) _partnerToken = "4894673879"; // "9999999999";
        }

        public void Save()
        {
            if (!_conf.Sections.ContainsKey("mp3tunes") || _conf.Sections["mp3tunes"] == null)
                _conf.Sections.Add("mp3tunes", new Dictionary<string, string>());
            _conf.Sections["mp3tunes"]["username"] = usernameTextbox.Text;
            _conf.Sections["mp3tunes"]["password"] = passwordTextbox.Text;
            _conf.Sections["mp3tunes"]["partner_token"] = _partnerToken;
            _conf.Save();

            // Reset the original values to the new saved values.
            _origUsername = usernameTextbox.Text;
            _origPassword = passwordTextbox.Text;

            FormContainer.Changed = false;

            _plugin.Host.ApplyChangesRequiresDaemonRestart();
        }

        public void Cancel()
        {
            usernameTextbox.Text = !string.IsNullOrEmpty(_origUsername) ? _origUsername : "";
            passwordTextbox.Text = !string.IsNullOrEmpty(_origPassword) ? _origPassword : "";
        }

        void mp3tunesLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mp3tunes.com/");
        }

        void usernameTextbox_TextChanged(object sender, System.EventArgs e)
        {
            if (FormContainer != null)
                FormContainer.Changed = !usernameTextbox.Text.Equals(_origUsername);
        }

        void passwordTextbox_TextChanged(object sender, System.EventArgs e)
        {
            if (FormContainer != null)
                FormContainer.Changed = !passwordTextbox.Text.Equals(_origPassword);
        }

        bool Completed()
        {
            return usernameTextbox.Text != "" && passwordTextbox.Text != "";
        }

        bool Changed()
        {
            return !usernameTextbox.Text.Equals(_origUsername)
                   || !passwordTextbox.Text.Equals(_origPassword);
        }

        private void usernameTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && Completed() && Changed()) Save();
        }

        private void passwordTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && Completed() && Changed()) Save();
        }

        private void usernameTextbox_Enter(object sender, System.EventArgs e)
        {
            usernameTextbox.SelectAll();
        }

        private void passwordTextbox_Enter(object sender, System.EventArgs e)
        {
            passwordTextbox.SelectAll();
        }
    }
}
