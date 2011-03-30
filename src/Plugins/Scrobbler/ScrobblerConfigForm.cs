/*
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010, 2011 Steven Robertson <steve@playnode.com>
 *
 * Windar - Playdar for Windows
 *
 * Windar is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License (LGPL) as published
 * by the Free Software Foundation; either version 2.1 of the License, or (at
 * your option) any later version.
 *
 * Windar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License version 2.1 for more details
 * (a copy is included in the LICENSE file that accompanied this code).
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Windows.Forms;
using Windar.Common;
using Windar.PluginAPI;

namespace Windar.ScrobblerPlugin
{
    public partial class ScrobblerConfigForm : UserControl, IConfigForm
    {
        IConfigFormContainer _formContainer;

        public IConfigFormContainer FormContainer
        {
            get { return _formContainer; }
            set { _formContainer = value; }
        }

        readonly ScrobblerPlugin _plugin;
        
        Credentials _creds;

        public ScrobblerConfigForm(ScrobblerPlugin plugin)
        {
            InitializeComponent();
            _plugin = plugin;
        }

        void ScrobblerConfigForm_Load(object sender, System.EventArgs e)
        {
            _creds = _plugin.Host.ScrobblerCredentials;
            usernameTextbox.Text = _creds.Username;
            passwordTextbox.Text = _creds.Password;
        }

        public void Save()
        {
            _creds.Username = usernameTextbox.Text;
            _creds.Password = passwordTextbox.Text;
            _plugin.Host.ScrobblerCredentials = _creds;

            FormContainer.Changed = false;

            _plugin.Host.ApplyChangesRequiresDaemonRestart();
        }

        public void Cancel()
        {
            usernameTextbox.Text = _creds.Username;
            passwordTextbox.Text = _creds.Password;
        }

        void usernameTextbox_TextChanged(object sender, System.EventArgs e)
        {
            if (FormContainer != null)
                FormContainer.Changed = !usernameTextbox.Text.Equals(_creds.Username);
        }

        void passwordTextbox_TextChanged(object sender, System.EventArgs e)
        {
            if (FormContainer != null)
                FormContainer.Changed = !passwordTextbox.Text.Equals(_creds.Password);
        }

        bool Completed()
        {
            return usernameTextbox.Text != "" && passwordTextbox.Text != "";
        }

        bool Changed()
        {
            return !usernameTextbox.Text.Equals(_creds.Username)
                   || !passwordTextbox.Text.Equals(_creds.Password);
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
