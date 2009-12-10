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

using System;
using System.Reflection;
using log4net;
using Windar.Common;
using Windar.PlaydarController.Commands;

namespace Windar.PlaydarController
{
    public class DaemonController
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        #region Delegates and events.

        public delegate void PlaydarStartedHandler(object sender, EventArgs e);
        public delegate void PlaydarStartFailedHandler(object sender, EventArgs e);
        public delegate void PlaydarStoppedHandler(object sender, EventArgs e);
        public delegate void ScanCompletedHandler(object sender, EventArgs e);

        public event PlaydarStartedHandler PlaydarStarted;
        public event PlaydarStartFailedHandler PlaydarStartFailed;
        public event PlaydarStoppedHandler PlaydarStopped;
        public event ScanCompletedHandler ScanCompleted;

        #endregion

        #region Properties

        internal static DaemonController Instance { get; private set; }

        internal WindarPaths Paths { get; private set; }

        public bool Started { get; private set; }

        public int NumFiles
        {
            get
            {
                var result = Cmd<NumFiles>.Create().Run();
                if (Log.IsDebugEnabled) Log.Debug("NumFiles result = " + result);
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
            Paths = new WindarPaths(appPath);
            Instance = this;
            Started = false;
        }

        #region Commands

        public void Start()
        {
            var cmd = Cmd<Start>.Create();
            cmd.PlaydarStarted += StartCmd_PlaydarStarted;
            cmd.PlaydarStartFailed += StartCmd_PlaydarStartFailed;
            cmd.RunAsync();
        }

        public void Stop()
        {
            Cmd<Stop>.Create().Run();
            PlaydarStopped(this, new EventArgs());
        }

        public void Restart()
        {
            Stop();
            System.Threading.Thread.Sleep(2000);
            Start();
        }

        public string Ping()
        {
            return Cmd<Ping>.Create().Run();
        }

        public void AddScanFileOrFolder(string path)
        {
            var cmd = Cmd<Scan>.Create();
            cmd.ScanCompleted += ScanCmd_ScanCompleted;
            cmd.ScanPath = path;
            cmd.RunAsync();
        }

        #endregion

        #region Command event handlers.

        private void StartCmd_PlaydarStarted(object sender, EventArgs e)
        {
            Started = true;
            PlaydarStarted(this, e);
        }

        private void StartCmd_PlaydarStartFailed(object sender, EventArgs e)
        {
            Started = false;
            PlaydarStartFailed(this, e);
        }

        private void ScanCmd_ScanCompleted(object sender, EventArgs e)
        {
            ScanCompleted(this, e);
        }

        #endregion
    }
}
