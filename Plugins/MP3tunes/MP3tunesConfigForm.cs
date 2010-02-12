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

using System.IO;
using System.Text;
using System.Windows.Forms;
using Windar.PluginAPI;

namespace Windar.MP3tunes
{
    public partial class MP3tunesConfigForm : UserControl, IConfigForm
    {
        public IConfigFormContainer FormContainer { private get; set; }

        readonly MP3tunesPlugin _plugin;

        string _origUsername;
        string _origPassword;
        string _origToken;

        #region Configuration filename.

        string _configFilename;
        string ConfigFilename
        {
            get
            {
                return _configFilename ??
                    (_configFilename = _plugin.Host.Paths.PlaydarEtcPath + @"\mp3tunes.conf");
            }
        }

        #endregion

        public MP3tunesConfigForm(MP3tunesPlugin plugin)
        {
            InitializeComponent();
            _plugin = plugin;
            var newFile = false;

            // Check if the configuration file already exists.
            if (File.Exists(ConfigFilename))
            {
                // Load config from file.
                var streamReader = new StreamReader(ConfigFilename);
                var line1 = streamReader.ReadLine();
                if (!line1.Equals("[mp3tunes]")) newFile = true;
                else
                {
                    var line2 = streamReader.ReadLine();
                    if (!line2.StartsWith("username=")) newFile = true;
                    else
                    {
                        var line3 = streamReader.ReadLine();
                        if (!line3.StartsWith("password=")) newFile = true;
                        else
                        {
                            var line4 = streamReader.ReadLine();
                            if (!line4.StartsWith("partner_token=")) newFile = true;
                            else
                            {
                                _origUsername = line2.Substring(9, line2.Length - 9);
                                _origPassword = line3.Substring(9, line3.Length - 9);
                                _origToken = line4.Substring(14, line4.Length - 14);
                            }
                        }
                    }
                }
                streamReader.Close();
            }
            if (!newFile) return;
            _origPassword = "";
            _origUsername = "";
            _origToken = "";
        }

        private void MP3tunesConfigForm_Load(object sender, System.EventArgs e)
        {
            usernameTextbox.Text = _origUsername;
            passwordTextbox.Text = _origPassword;
            if (string.IsNullOrEmpty(_origToken)) _origToken = "9999999999";
            tokenTextbox.Text = _origToken;
        }

        public void Save()
        {
            var str = new StringBuilder();
            str.Append("[mp3tunes]\n");
            str.Append("username=").Append(usernameTextbox.Text).Append("\n");
            str.Append("password=").Append(passwordTextbox.Text).Append("\n");
            str.Append("partner_token=").Append(tokenTextbox.Text).Append("\n");

            // Write the configuration file.
            if (File.Exists(ConfigFilename)) File.Delete(ConfigFilename);
            var file = new FileStream(ConfigFilename, FileMode.Create, FileAccess.Write);
            var sw = new StreamWriter(file);
            sw.Write(str.ToString());
            sw.Close();
            file.Close();

            _origUsername = usernameTextbox.Text;
            _origPassword = passwordTextbox.Text;
            _origToken = tokenTextbox.Text;

            FormContainer.Changed = false;
        }

        public void Cancel()
        {
            usernameTextbox.Text = _origUsername;
            passwordTextbox.Text = _origPassword;
            tokenTextbox.Text = _origToken;
        }

        private void tokensLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mp3tunes.com/partner/cb/tokens");
        }

        private void mp3tunesLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mp3tunes.com/");
        }

        private void usernameTextbox_TextChanged(object sender, System.EventArgs e)
        {
            if (FormContainer != null)
                FormContainer.Changed = !usernameTextbox.Text.Equals(_origUsername);
        }

        private void passwordTextbox_TextChanged(object sender, System.EventArgs e)
        {
            if (FormContainer != null)
                FormContainer.Changed = !passwordTextbox.Text.Equals(_origPassword);
        }

        private void tokenTextbox_TextChanged(object sender, System.EventArgs e)
        {
            if (FormContainer != null)
                FormContainer.Changed = !tokenTextbox.Text.Equals(_origToken);
        }
    }
}
