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

namespace Windar.Common
{
    public abstract class ShortCmd<T> : Cmd<T> where T : new()
    {
        private readonly StringBuilder _stdOutput;
        private readonly StringBuilder _stdErr;

        #region Properties

        public string Output
        {
            get { return _stdOutput.ToString(); }
        }

        public string Error
        {
            get { return _stdErr.ToString(); }
        }

        #endregion

        protected ShortCmd()
        {
            _stdOutput = new StringBuilder();
            _stdErr = new StringBuilder();
            Runner.CommandOutput += Cmd_CommandOutput;
            Runner.CommandError += Cmd_CommandError;
        }

        public abstract string Run();

        protected void Cmd_CommandOutput(object sender, CmdRunner.CommandEventArgs e)
        {
            _stdOutput.Append(e.Text).Append('\n');
        }

        protected void Cmd_CommandError(object sender, CmdRunner.CommandEventArgs e)
        {
            _stdErr.Append(e.Text).Append('\n');
        }
    }
}
