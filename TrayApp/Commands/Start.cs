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

using System.Text;

namespace Windar.Commands
{
    class Start : Cmd<Start>
    {
        public void RunAsync()
        {
            Cmd<CopyAppFilesToAppData>.Create().Run();
            Runner.RunCommand(@"cd " + Paths.PlaydarDataPath);
            var cmd = new StringBuilder();
            cmd.Append('"').Append(Paths.ErlCmd).Append('"');
            cmd.Append(" -sname playdar@localhost");
            cmd.Append(" -noinput");
            cmd.Append(" -pa \"").Append(Paths.PlaydarPath).Append("\\ebin\"");
            cmd.Append(" -boot start_sasl");
            cmd.Append(" -s reloader");
            cmd.Append(" -s playdar");
            Runner.RunCommand(cmd.ToString());
            Program.Instance.ShowTrayInfo("Playdar started.");
        }
    }
}
