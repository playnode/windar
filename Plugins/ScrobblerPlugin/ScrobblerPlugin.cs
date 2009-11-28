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

using System.Reflection;
using log4net;
using Windar.PluginAPI;

namespace Windar.ScrobblerPlugin
{
    public class ScrobblerPlugin : IPlugin
    {
        #region Properties

        public IPluginHost Host { private get; set; }

        public string Name
        {
            get
            {
                return "Scrobbler";
            }
        }

        public string Description
        {
            get
            {
                return "Provides audio scrobbler support via a plugin.";
            }
        }

        #endregion

        public void Load()
        {
            //TODO: Add properties (username, password) to options window.
        }

        public void Shutdown()

        {
        }
    }
}
