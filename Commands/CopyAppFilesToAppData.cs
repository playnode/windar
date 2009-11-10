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

using System.Text;

namespace Windar.Commands
{
    class CopyAppFilesToAppData : Cmd<CopyAppFilesToAppData>
    {
        public string Run()
        {
            // 'etc' folder, containing config file.
            var cmd = new StringBuilder();
            cmd.Append("if not exist \"").Append(Paths.PlaydarDataPath).Append("\\etc\" ");
            cmd.Append("xcopy /e \"").Append(Paths.PlaydarPath).Append("\\etc\"");
            cmd.Append(" \"").Append(Paths.PlaydarDataPath).Append("\\etc\\\"");
            Runner.RunCommand(cmd.ToString());

            // 'priv' folder, containing web files.
            cmd = new StringBuilder();
            cmd.Append("if not exist \"").Append(Paths.PlaydarDataPath).Append("\\priv\" ");
            cmd.Append("xcopy /e \"").Append(Paths.PlaydarPath).Append("\\priv\"");
            cmd.Append(" \"").Append(Paths.PlaydarDataPath).Append("\\priv\\\"");
            Runner.RunCommand(cmd.ToString());

            // 'playdar_modules' folder, containing web files.
            cmd = new StringBuilder();
            cmd.Append("if not exist \"").Append(Paths.PlaydarDataPath).Append("\\playdar_modules\" ");
            cmd.Append("xcopy /e \"").Append(Paths.PlaydarPath).Append("\\playdar_modules\"");
            cmd.Append(" \"").Append(Paths.PlaydarDataPath).Append("\\playdar_modules\\\"");
            Runner.RunCommand(cmd.ToString());

            return WhenDone();
        }
    }
}
