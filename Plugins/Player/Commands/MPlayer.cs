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

using System;
using System.Reflection;
using System.Text;
using log4net;
using Windar.Common;

namespace Windar.PlayerPlugin.Commands
{
    /// <summary>
    /// Uses the slave mode in MPlayer to provide the player.
    /// http://www.mplayerhq.hu/DOCS/tech/slave.txt
    /// http://www.mplayerhq.hu/DOCS/man/en/mplayer.1.txt
    /// 
    /// State changes 
    /// </summary>
    class MPlayer : AsyncCmd<MPlayer>
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        #region State

        public enum State
        {
            Initial,
            Running,
            Caching,
            Playing,
            Paused,
            Stopped,
            Error,
            Ended
        }

        public State PlayerState = State.Initial;

        void ChangeState(State to)
        {
            ChangeState(to, null);
        }

        void ChangeState(State to, string msg)
        {
            if (Log.IsDebugEnabled) Log.Debug("Changing state to " + to);
            PlayerState = to;
            _page.StateChanged(to, msg);
        }

        #endregion

        readonly string _filename;
        readonly PlayItem _item;
        private readonly PlayerTabPage _page;

        #region Progress & volume properties.

        int _progress;
        public int Progress
        {
            get
            {
                // Send command to issue progress.
                switch (PlayerState)
                {
                    case State.Playing:
                    case State.Paused:
                    case State.Caching:
                        Runner.Process.StandardInput.WriteLine("\nget_percent_pos\n");
                        Runner.Process.StandardInput.Flush();
                        break;
                    default:
                        if (Log.IsWarnEnabled)
                            Log.Warn("Position request with unsupported state = " + PlayerState);
                        break;
                }

                // Return current value.
                // This is probably from the last progress request.
                return _progress;
            }
        }

        int _volume = 100;
        public int Volume
        {
            set
            {
                _volume = value;
                switch (PlayerState)
                {
                    case State.Playing:
                    case State.Caching:
                        var str = new StringBuilder();
                        str.Append("\nvolume ").Append(_volume).Append(" 1\n");
                        Runner.Process.StandardInput.WriteLine(str.ToString());
                        Runner.Process.StandardInput.Flush();
                        break;
                }
            }
        }

        #endregion

        #region Constructor

        public MPlayer()
        {
            // Empty constructor required for using generics.
            // DO NOT USE.
        }

        public MPlayer(PlayItem item, string filename, PlayerTabPage page)
        {
            _filename = filename;
            _item = item;
            _page = page;
        }

        #endregion

        /// <summary>
        /// This method is used to play a stream.
        /// </summary>
        public override void RunAsync()
        {
            Runner.CommandCompleted += CommandCompleted;
            Runner.CommandOutput += CommandOutput;
            Runner.CommandError += CommandError;

            var cmd = new StringBuilder();
            cmd.Append('"').Append(_page.Plugin.Host.ProgramFilesPath);
            cmd.Append(@"mplayer\").Append("mplayer.exe\"");
            cmd.Append(" -slave");
            cmd.Append(" -quiet");
            cmd.Append(" -volume ").Append(_volume);
            cmd.Append(' ').Append(_filename);

            Runner.RunCommand(cmd.ToString());
            ChangeState(State.Running);
        }

        #region Player control methods.

        public void Pause()
        {
            if (PlayerState != State.Playing)
            {
                if (Log.IsWarnEnabled) 
                    Log.Warn("Invalid state change. Not playing.");
            }
            else
            {
                if (Log.IsDebugEnabled) Log.Debug("Trying to pause.");
                Runner.Process.StandardInput.WriteLine("\npause\n");
                Runner.Process.StandardInput.Flush();
                ChangeState(State.Paused);
            }
        }

        public void Resume()
        {
            if (PlayerState != State.Paused)
            {
                if (Log.IsWarnEnabled) 
                    Log.Warn("Invalid state change. Not paused.");
            }
            else
            {
                if (Log.IsDebugEnabled) Log.Debug("Trying to resume.");
                Runner.Process.StandardInput.WriteLine("\npause\n");
                Runner.Process.StandardInput.Flush();
                ChangeState(State.Playing);
            }
        }

        public void Stop()
        {
            if (PlayerState == State.Stopped)
            {
                if (Log.IsWarnEnabled) Log.Warn("Invalid state change. Already stopped.");
            }
            else
            {
                Runner.Process.StandardInput.Flush();
                Runner.Process.StandardInput.WriteLine("\nstop\n");
                Runner.Process.StandardInput.Flush();
                ChangeState(State.Stopped);
            }
        }

        #endregion

        #region Command event handlers.

        protected void CommandOutput(object sender, CmdRunner.CommandEventArgs e)
        {
            if (e.Text.StartsWith("Cache fill:"))
            {
                ChangeState(State.Caching, e.Text);
            }
            else if (e.Text.Equals("Starting playback..."))
            {
                ChangeState(State.Playing, PlayerPlugin.GetPlayingMessage(_item));
            }
            else if (e.Text.Equals("Exiting... (End of file)"))
            {
                if (Log.IsInfoEnabled) Log.Info(e.Text);
                ChangeState(State.Stopped);
            }
            else if (e.Text.StartsWith("ANS_PERCENT_POSITION="))
            {
                _progress = Convert.ToInt32(e.Text.Substring(21));
                if (_progress == 100) ChangeState(State.Ended);
            }
            else if (Log.IsDebugEnabled)
            {
                Log.Debug(e.Text);
            }
        }

        protected void CommandError(object sender, CmdRunner.CommandEventArgs e)
        {
            // Ignore errors if player is stopped.
            var ignore = false;
            switch (PlayerState)
            {
                case State.Ended:
                    ignore = true;
                    break;
                case State.Stopped:
                    ignore = true;
                    break;
                default:
                    if (e.Text.Equals("Fontconfig error: Cannot load default config file")) 
                        ignore = true;
                    else if (e.Text.Equals("Failed, exiting."))
                        ChangeState(State.Error, "Failed. Play cancelled.");
                    else
                        ChangeState(State.Error, e.Text);
                    break;
            }

            if (!Log.IsErrorEnabled) return;
            if (ignore) Log.Error("Ignoring error: " + e.Text);
            else Log.Error(e.Text);
        }

        protected void CommandCompleted(object sender, EventArgs e)
        {
            ChangeState(State.Ended);
        }

        #endregion
    }
}
