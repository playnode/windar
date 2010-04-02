/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
 *
 * Windar: Playdar for Windows
 *
 * Windar is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
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

using Windar.PluginAPI;

namespace Windar.MP3tunes
{
    public class MP3tunesPlugin : IPlugin
    {
        public IPluginHost Host { get; set; }

        public string Name
        {
            get
            {
                return "MP3tunes";
            }
        }

        public void Load()
        {
            Host.AddConfigurationPage(new ConfigTabContent(new MP3tunesConfigForm(this)), Name);
        }

        public void Shutdown()
        {
        }
    }
}
