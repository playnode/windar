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

using System.Reflection;
using log4net;
using Windar.PluginAPI;

namespace Windar.PlayerPlugin
{
    public class ElectronomePlugin : IPlugin
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        #region Properties

        public IPluginHost Host { get; set; }
        public Electronome Electronome { get; set; }

        public string Name
        {
            get
            {
                return "Electronome";
            }
        }

        public string Description
        {
            get
            {
                return "Music discovery.";
            }
        }

        #endregion

        public ElectronomePlugin()
        {
            Electronome = new Electronome();
        }

        public void Load()
        {
            Host.AddTabPage(Electronome, Name);
        }

        public void Shutdown()
        {

        }
    }
}
