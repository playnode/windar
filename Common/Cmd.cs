﻿/*
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
using System.Threading;
using log4net;

namespace Windar.Common
{
    public abstract class Cmd<T> where T : new()
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        protected CmdRunner Runner { get; private set; }

        protected bool Done { get; private set; }

        protected Cmd()
        {
            Runner = new CmdRunner();
            Runner.CommandCompleted += Cmd_CommandCompleted;
        }

        public static T Create()
        {
            return new T();
        }

        protected void ContinueWhenDone()
        {
            if (Log.IsDebugEnabled) Log.Debug("Continue when done...");
            while (!Done) Thread.Sleep(100);
            if (Log.IsDebugEnabled) Log.Debug("Done.");
            Runner.Close();
            if (Log.IsDebugEnabled) Log.Debug("Runner closed.");
        }

        protected void Cmd_CommandCompleted(object sender, EventArgs e)
        {
            Done = true;
        }
    }
}
