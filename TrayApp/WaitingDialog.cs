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

using System.Windows.Forms;

namespace Windar.TrayApp
{
    public partial class WaitingDialog : Form
    {
        public delegate void DoMethod();
        
        public DoMethod Do { get; set; }

        private bool _run;

        public WaitingDialog()
        {
            InitializeComponent();
        }

        public void Stop()
        {
            _run = false;
        }

        private void WaitingDialog_Shown(object sender, System.EventArgs e)
        {
            _run = true;
            if (Do != null) backgroundWorker.RunWorkerAsync();
            while (_run)
            {
                progressBar.Update();
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
            }
            Close();
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Do();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _run = false;
        }
    }
}
