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
using System.Text;

namespace Windar.Commands
{
    class Scan : Cmd<Scan>
    {
        private bool _firstRun;

        public void RunAsync(string path)
        {
            if (path == null) throw new ApplicationException("Path must be defined");

            Runner.CommandCompleted += Completed;
            Runner.RunCommand(@"cd " + Paths.PlaydarDataPath);
            var cmd = new StringBuilder();
            cmd.Append('"').Append(Paths.ErlCmd).Append('"');
            cmd.Append(" -sname windar-scan@localhost");
            cmd.Append(" -noinput");
            cmd.Append(" -pa \"").Append(Paths.PlaydarPath).Append("\\ebin\"");
            cmd.Append(" -s playdar_ctl");
            cmd.Append(" -extra playdar@localhost \"scan\" \"").Append(path).Append("\"");
            Runner.RunCommand(cmd.ToString());
        }

        protected void Completed(object sender, CmdRunner.CommandEventArgs e)
        {
            //NOTE: This event first at start for some as yet unknown reason. Ignore that for now.
            if (!_firstRun) _firstRun = true;
            else Program.Instance.ShowTrayInfo("Scan completed.");
        }
    }
}
