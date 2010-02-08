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
using System.Text;
using Windar.Common;

namespace Windar.PlayerPlugin.Commands
{
    class Play : AsyncCmd<Play>
    {
        public delegate void PlayCompletedHandler(object sender, EventArgs e);

        public event PlayCompletedHandler PlayCompleted;

        public string Filename { get; set; }
        public Player Player { get; set; }

        public override void RunAsync()
        {
            if (Filename == null) throw new ApplicationException("Filename must be defined");
            if (Player == null) throw new ApplicationException("Player must be defined");

            Runner.CommandCompleted += Completed;
            Player.PausePlayer += Pause;
            Player.ResumePlayer += Resume;
            Player.StopPlayer += Stop;

            var cmd = new StringBuilder();
            cmd.Append('"').Append(Player.Page.Plugin.Host.ProgramFilesPath);
            cmd.Append(@"mplayer\").Append("mplayer.exe\" -slave -quiet ");
            cmd.Append(Filename);

            //cmd.Append(" -cache 8192");

            Runner.RunCommand(cmd.ToString());
        }

        protected void Pause(object sender, EventArgs e)
        {
            Runner.Process.StandardInput.WriteLine("\npause\n");
            Runner.Process.StandardInput.Flush();
        }

        protected void Resume(object sender, EventArgs e)
        {
            Runner.Process.StandardInput.WriteLine("\npause\n");
            Runner.Process.StandardInput.Flush();
        }

        protected void Stop(object sender, EventArgs e)
        {
            Runner.Process.StandardInput.WriteLine("\npause\n");
            Runner.Process.StandardInput.Flush();
            Runner.Process.StandardInput.WriteLine("\nstop\n");
            Runner.Process.StandardInput.Flush();
        }

        protected void Completed(object sender, EventArgs e)
        {
            if (PlayCompleted != null)
                PlayCompleted(this, new EventArgs());
        }
    }
}
