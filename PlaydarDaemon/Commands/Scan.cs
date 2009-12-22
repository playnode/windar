/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009 Steven Robertson <steve@playnode.org>
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

using System;
using System.Text;
using Windar.Common;

namespace Windar.PlaydarDaemon.Commands
{
    class Scan : AsyncCmd<Scan>
    {
        public delegate void ScanCompletedHandler(object sender, EventArgs e);

        public event ScanCompletedHandler ScanCompleted;

        private bool _firstRun;

        public string ScanPath { get; set; }

        public override void RunAsync()
        {
            if (ScanPath == null) throw new ApplicationException("Path must be defined");

            Runner.CommandCompleted += Completed;

            Runner.RunCommand(@"CD " + DaemonController.Instance.Paths.PlaydarDataPath);

            var cmd = new StringBuilder();
            cmd.Append('"').Append(DaemonController.Instance.Paths.ErlCmd).Append('"');
            cmd.Append(" -sname playdar-scan@localhost");
            cmd.Append(" -noinput");
            cmd.Append(" -pa \"").Append(DaemonController.Instance.Paths.PlaydarPath).Append("\\ebin\"");
            cmd.Append(" -s playdar_ctl");
            cmd.Append(" -extra playdar@localhost \"scan\" \"");
            cmd.Append(ScanPath).Append("\"");

            Runner.RunCommand(cmd.ToString());
        }

        protected void Completed(object sender, EventArgs e)
        {
            //NOTE: This event first at start for some reason. Ignore first.
            if (!_firstRun) _firstRun = true;
            else ScanCompleted(this, new EventArgs());
        }
    }
}
