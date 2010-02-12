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

using System.Windows.Forms;
using Windar.PluginAPI;

namespace Windar.ScrobblerPlugin
{
    public partial class ScrobblerConfigForm : UserControl, IConfigForm
    {
        public IConfigFormContainer FormContainer { get; set; }

        readonly ScrobblerPlugin _plugin;

        string _origUsername;
        string _origPassword;

        public ScrobblerConfigForm(ScrobblerPlugin plugin)
        {
            InitializeComponent();
            _plugin = plugin;

            //TODO: Load existing configuration.
            _origPassword = "";
            _origUsername = "";
        }

        void ScrobblerConfigForm_Load(object sender, System.EventArgs e)
        {

        }

        public void Save()
        {
            //TODO: Save config.

            _origUsername = usernameTextbox.Text;
            _origPassword = passwordTextbox.Text;

            FormContainer.Changed = false;

            _plugin.Host.ApplyChangesRequiresDaemonRestart();
        }

        public void Cancel()
        {
            usernameTextbox.Text = _origUsername;
            passwordTextbox.Text = _origPassword;
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
    }
}
