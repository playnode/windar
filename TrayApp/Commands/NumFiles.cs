/*
 * Windar: Playdar for Windows
 * Copyright (C) 2009 Steven Robertson <http://stever.org.uk/>
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
    class NumFiles : Cmd<NumFiles>
    {
        public string Run()
        {
            Runner.RunCommand(@"cd " + Paths.PlaydarDataPath);
            var cmd = new StringBuilder();
            cmd.Append('"').Append(Paths.ErlCmd).Append('"');
            cmd.Append(" -sname windar-scan@localhost");
            cmd.Append(" -noinput");
            cmd.Append(" -pa \"").Append(Paths.PlaydarPath).Append("\\ebin\"");
            cmd.Append(" -s playdar_ctl");
            cmd.Append(" -extra playdar@localhost \"numfiles\"");
            Runner.RunCommand(cmd.ToString());
            var result = WhenDone();
            try
            {
                var n = Int32.Parse(result);
                if (n == 1) result = "There is one file in your Playdar library.";
                else result = "There are " + n + " files in your Playdar library.";
            }
            catch (FormatException)
            {
                // Ignore. Returning the string result.
            }
            return result;
        }
    }
}
