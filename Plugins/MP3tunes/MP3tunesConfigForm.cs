/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
 *
 * Windar: Playdar for Windows
 *
 * Windar is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
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
        string _origPartnerToken;

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
                _origPartnerToken = !_conf.Sections["mp3tunes"].ContainsKey("partner_token") 
                    ? null : _conf.Sections["mp3tunes"]["partner_token"];
            }
            else
            {
                _origUsername = null;
                _origPassword = null;
                _origPartnerToken = null;
            }
        }

        void MP3tunesConfigForm_Load(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(_origUsername)) usernameTextbox.Text = _origUsername;
            if (!string.IsNullOrEmpty(_origPassword)) passwordTextbox.Text = _origPassword;
            if (string.IsNullOrEmpty(_origPartnerToken)) _origPartnerToken = "4894673879"; // "9999999999";
            tokenTextbox.Text = _origPartnerToken;
        }

        public void Save()
        {
            if (!_conf.Sections.ContainsKey("mp3tunes") || _conf.Sections["mp3tunes"] == null)
                _conf.Sections.Add("mp3tunes", new Dictionary<string, string>());
            _conf.Sections["mp3tunes"]["username"] = usernameTextbox.Text;
            _conf.Sections["mp3tunes"]["password"] = passwordTextbox.Text;
            _conf.Sections["mp3tunes"]["partner_token"] = tokenTextbox.Text;
            _conf.Save();

            // Reset the original values to the new saved values.
            _origUsername = usernameTextbox.Text;
            _origPassword = passwordTextbox.Text;
            _origPartnerToken = tokenTextbox.Text;

            FormContainer.Changed = false;

            _plugin.Host.ApplyChangesRequiresDaemonRestart();
        }

        public void Cancel()
        {
            usernameTextbox.Text = !string.IsNullOrEmpty(_origUsername) ? _origUsername : "";
            passwordTextbox.Text = !string.IsNullOrEmpty(_origPassword) ? _origPassword : "";
            tokenTextbox.Text = !string.IsNullOrEmpty(_origPartnerToken) ? _origPartnerToken : "";
        }

        void tokensLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mp3tunes.com/partner/cb/tokens");
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

        void tokenTextbox_TextChanged(object sender, System.EventArgs e)
        {
            if (FormContainer != null)
                FormContainer.Changed = !tokenTextbox.Text.Equals(_origPartnerToken);
        }
    }
}
