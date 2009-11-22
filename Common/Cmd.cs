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
using System.Threading;

namespace Windar.Common
{
    public abstract class Cmd<T> where T : new()
    {
        protected CmdRunner Runner { get; private set; }

        protected bool Done { get; private set; }

        private readonly StringBuilder _stdOutput;
        private readonly StringBuilder _stdErr;

        protected Cmd()
        {
            Runner = new CmdRunner();

            _stdOutput = new StringBuilder();
            _stdErr = new StringBuilder();

            Runner.CommandOutput += Cmd_CommandOutput;
            Runner.CommandError += Cmd_CommandError;
            Runner.CommandCompleted += Cmd_CommandCompleted;

            //NOTE: Following is not currently required as etc is found in current dir.
            //Runner.RunCommand("set PLAYDAR_ETC=" + Paths.PlaydarDataPath + @"\etc");
        }

        public static T Create()
        {
            return new T();
        }

        protected string WhenDone()
        {
            while (!Done) Thread.Sleep(100);
            Runner.Close();
            return _stdOutput.ToString();
        }

        protected void Cmd_CommandOutput(object sender, CmdRunner.CommandEventArgs e)
        {
            _stdOutput.Append(e.Text);
        }

        protected void Cmd_CommandError(object sender, CmdRunner.CommandEventArgs e)
        {
            _stdErr.Append(e.Text);
        }

        protected void Cmd_CommandCompleted(object sender, CmdRunner.CommandEventArgs e)
        {
            Done = true;
        }
    }
}
