/*
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
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

namespace Windar.PluginAPI
{
    public partial class ConfigTabContent : UserControl, IConfigFormContainer
    {
        readonly IConfigForm _form;

        public bool Changed
        {
            set
            {
                configSaveButton.Enabled = value;
                configCancelButton.Enabled = value;
            }
        }

        public ConfigTabContent(IConfigForm form)
        {
            _form = form;
            InitializeComponent();

            // Reference container with the form to allow the state of the buttons
            // to be changed by the events on changes to the form.
            form.FormContainer = this;

            // Anchor the form to the four sides of the container.
            configContentPanel.Controls.Add((Control) _form);
            ((Control) _form).Anchor = AnchorStyles.Bottom |
                AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        }

        protected ConfigTabContent()
        {
            InitializeComponent();
        }

        void configSaveButton_Click(object sender, System.EventArgs e)
        {
            _form.Save();
        }

        void configCancelButton_Click(object sender, System.EventArgs e)
        {
            _form.Cancel();
        }
    }
}
