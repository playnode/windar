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

using System.Windows.Forms;

namespace Windar.TrayApp
{
    public partial class WaitingDialog : Form
    {
        public delegate void DoMethod();
        
        public DoMethod Do { get; set; }

        bool _run;

        public WaitingDialog()
        {
            InitializeComponent();
        }

        void SetRun(bool run)
        {
            _run = run;
            Program.Instance.Tray.ToggleMainFormOptions(!_run);
        }

        public void Stop()
        {
            SetRun(false);
        }

        void WaitingDialog_Shown(object sender, System.EventArgs e)
        {
            SetRun(true);
            if (Do != null) backgroundWorker.RunWorkerAsync();
            while (_run)
            {
                progressBar.Update();
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
            }
            Close();
        }

        void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Do();
        }

        void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            SetRun(false);
        }
    }
}
