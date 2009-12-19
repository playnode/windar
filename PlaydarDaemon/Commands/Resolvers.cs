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

using System.Text;
using Windar.Common;

namespace Windar.PlaydarDaemon.Commands
{
    class Resolvers : ShortCmd<Resolvers>
    {
        public override string Run()
        {
            Runner.RunCommand("CD " + DaemonController.Instance.Paths.PlaydarPath);
            Runner.RunCommand("SET PLAYDAR_ETC=" + DaemonController.Instance.Paths.PlaydarDataPath + @"\etc");

            var cmd = new StringBuilder();
            cmd.Append('"').Append(DaemonController.Instance.Paths.ErlCmd).Append('"');
            cmd.Append(" -sname playdar-resolvers@localhost");
            cmd.Append(" -noinput");
            cmd.Append(" -pa \"").Append(DaemonController.Instance.Paths.PlaydarPath).Append("\\ebin\"");
            cmd.Append(" -s playdar_ctl");
            cmd.Append(" -extra playdar@localhost \"resolvers\"");

            Runner.SkipLogInfoOutput = true;
            Runner.RunCommand(cmd.ToString());
            ContinueWhenDone();
            return Output;
        }
    }
}
