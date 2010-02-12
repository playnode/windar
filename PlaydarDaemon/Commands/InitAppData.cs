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

using System.Text;
using Windar.Common;

namespace Windar.PlaydarDaemon.Commands
{
    class InitAppData : ShortCmd<InitAppData>
    {
        public override string Run()
        {
            Runner.SkipLogInfoOutput = false;

            // Path to %AppData%\Windar2
            var cmd = new StringBuilder();
            cmd.Append("IF NOT EXIST \"").Append(DaemonController.Instance.Paths.WindarAppData).Append("\" ");
            cmd.Append("MKDIR \"").Append(DaemonController.Instance.Paths.WindarAppData).Append("\\\"");
            Runner.RunCommand(cmd.ToString());

            // Path to %AppData%\Windar2\etc
            cmd = new StringBuilder();
            cmd.Append("IF NOT EXIST \"").Append(DaemonController.Instance.Paths.WindarAppData).Append("\\etc\" ");
            cmd.Append("MKDIR \"").Append(DaemonController.Instance.Paths.WindarAppData).Append("\\etc\\\"");
            Runner.RunCommand(cmd.ToString());

            // Playdar configuration file.
            cmd = new StringBuilder();
            cmd.Append("IF NOT EXIST \"").Append(DaemonController.Instance.Paths.WindarAppData).Append("\\etc\\playdar.conf\" ");
            cmd.Append("COPY \"").Append(DaemonController.Instance.Paths.PlaydarProgramPath).Append("\\etc\\playdar.conf\"");
            cmd.Append(" \"").Append(DaemonController.Instance.Paths.WindarAppData).Append("\\etc\\\"");
            Runner.RunCommand(cmd.ToString());

            // Playdar TCP configuration file.
            cmd = new StringBuilder();
            cmd.Append("IF NOT EXIST \"").Append(DaemonController.Instance.Paths.WindarAppData).Append("\\etc\\playdartcp.conf\" ");
            cmd.Append("COPY \"").Append(DaemonController.Instance.Paths.PlaydarProgramPath).Append("\\etc\\playdartcp.conf\"");
            cmd.Append(" \"").Append(DaemonController.Instance.Paths.WindarAppData).Append("\\etc\\\"");
            Runner.RunCommand(cmd.ToString());

            ContinueWhenDone();
            return Output;
        }
    }
}
