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
using Windar.Common;
using Windar.PlaydarController.Commands;

namespace Windar.PlaydarController
{
    public class DaemonController
    {
        #region Delegates and events.

        public delegate void ScanCompletedHandler(object sender, EventArgs e);

        public event ScanCompletedHandler ScanCompleted;

        #endregion

        #region Properties

        internal static DaemonController Instance { get; private set; }

        private string ApplicationPath { get; set; }

        private string _erlPath;
        private string _erlCmd;
        private string _playdarPath;
        private string _playdarDataPath;

        internal string ErlPath
        {
            get { return _erlPath ?? (_erlPath = ApplicationPath + @"\minimerl"); }
        }

        internal string ErlCmd
        {
            get { return _erlCmd ?? (_erlCmd = ErlPath + @"\bin\erl.exe"); }
        }

        internal string PlaydarPath
        {
            get { return _playdarPath ?? (_playdarPath = ApplicationPath + @"\playdar"); }
        }

        internal string PlaydarDataPath
        {
            get { return _playdarDataPath ?? (_playdarDataPath = @"%AppData%\Windar"); }
        }

        public int NumFiles
        {
            get
            {
                var result = Cmd<NumFiles>.Create().Run();
                try
                {
                    return Int32.Parse(result);
                }
                catch (FormatException)
                {
                    //TODO: Try to create a useful error message.
                    throw new Exception(result);
                }
            }
        }

        #endregion

        public DaemonController(string appPath)
        {
            ApplicationPath = appPath;
            Instance = this;
        }

        public void Start()
        {
            Cmd<Start>.Create().RunAsync();
        }

        public void Stop()
        {
            Cmd<Stop>.Create().Run();
        }

        public void Restart()
        {
            Stop(); Start();
        }

        public string Ping()
        {
            return Cmd<Ping>.Create().Run();
        }

        public void AddScanFileOrFolder(string path)
        {
            var cmd = Cmd<Scan>.Create();
            cmd.ScanCompleted += ScanCmd_ScanCompleted;
            cmd.RunAsync(path);
        }

        private void ScanCmd_ScanCompleted(object sender, EventArgs e)
        {
            ScanCompleted(this, e);
        }
    }
}