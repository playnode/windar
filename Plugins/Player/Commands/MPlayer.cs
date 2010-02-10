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
    /// </summary>
    class MPlayer : AsyncCmd<MPlayer>
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

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

        readonly string _filename;
        readonly PlayItem _item;
        private readonly PlayerTabPage _page;

        public State PlayerState;

        int _volume;
        int _progress;

        public int Progress
        {
            get
            {
                RequestProgress();
                return _progress;
            }
        }

        public int Volume
        {
            set
            {
                _volume = value;
                switch (PlayerState)
                {
                    case State.Playing:
                    case State.Paused:
                    case State.Caching:
                        var str = new StringBuilder();
                        str.Append("\nvolume ").Append(_volume).Append(" 1\n");
                        Runner.Process.StandardInput.WriteLine(str.ToString());
                        Runner.Process.StandardInput.Flush();
                        break;
                }
            }
        }

        public MPlayer()
        {
            
        }

        public MPlayer(PlayItem item, string filename, PlayerTabPage page)
        {
            _volume = 100;
            _filename = filename;
            _item = item;
            _page = page;
            PlayerState = State.Initial;
        }

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
            PlayerState = State.Running;
        }

        public void Pause()
        {
            if (PlayerState != State.Playing)
            {
                if (Log.IsWarnEnabled) 
                    Log.Warn("Invalid state change. Not playing.");
            }
            else
            {
                Runner.Process.StandardInput.WriteLine("\npause\n");
                Runner.Process.StandardInput.Flush();
                PlayerState = State.Paused;
            }
            if (Log.IsDebugEnabled)
                Log.Debug("Player state = " + PlayerState);
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
                Runner.Process.StandardInput.WriteLine("\npause\n");
                Runner.Process.StandardInput.Flush();
                PlayerState = State.Playing;
            }
            if (Log.IsDebugEnabled)
                Log.Debug("Player state = " + PlayerState);
        }

        public void Stop()
        {
            if (PlayerState == State.Stopped)
            {
                if (Log.IsWarnEnabled) Log.Warn("Invalid state change. Already stopped.");
            }
            else
            {
                Runner.Process.StandardInput.WriteLine("\nstop\n");
                Runner.Process.StandardInput.Flush();
                PlayerState = State.Stopped;
            }
            if (Log.IsDebugEnabled)
                Log.Debug("Player state = " + PlayerState);
        }

        public void RequestProgress()
        {
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
        }

        protected void CommandCompleted(object sender, EventArgs e)
        {
            PlayerState = State.Ended;
        }

        protected void CommandOutput(object sender, CmdRunner.CommandEventArgs e)
        {
            if (Log.IsDebugEnabled) Log.Debug(e.Text);
            if (e.Text.Length > 11 && e.Text.Substring(0, 11).Equals("Cache fill:"))
            {
                _page.SetStatus(e.Text);
                PlayerState = State.Caching;
                _page.StateChanged();
            }
            else if (e.Text.Length == 20 && e.Text.Equals("Starting playback..."))
            {
                _page.SetStatus(PlayerPlugin.GetPlayingMessage(_item));
                PlayerState = State.Playing;
                _page.StateChanged();
            }
            else if (e.Text.Length == 24 && e.Text.Equals("Exiting... (End of file)"))
            {
                _page.Stopped();
                PlayerState = State.Stopped;
                _page.StateChanged();
            }
            else if (e.Text.Length >= 21 && e.Text.Substring(0, 21).Equals("ANS_PERCENT_POSITION="))
            {
                _progress = Convert.ToInt32(e.Text.Substring(21));
                if (Log.IsDebugEnabled) Log.Debug("Progress = " + _progress);
            }
        }

        protected void CommandError(object sender, CmdRunner.CommandEventArgs e)
        {
            if (Log.IsErrorEnabled) Log.Error(e.Text);

            // Ignore errors if player is stopped.
            if (PlayerState == State.Stopped) return;

            PlayerState = State.Error;
            if (e.Text.Length == 16 && e.Text.Equals("Failed, exiting."))
                _page.SetStatus("Failed. Play cancelled.");
            else
                _page.SetStatus("Error: " + e.Text);
        }
    }
}
