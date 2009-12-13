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
using System.Collections.Generic;

namespace Windar.TrayApp.Configuration
{
    class GeneralOptionsPage : IOptionsPage
    {
        public string NodeName
        {
            get
            {
                var result = Program.Instance.MainConfig.Name;
                if (string.IsNullOrEmpty(result)) result = Environment.MachineName;
                return result;
            }
            set { Program.Instance.MainConfig.Name = value; }
        }

        public int Port
        {
            get { return Program.Instance.MainConfig.WebPort; }
            set { Program.Instance.MainConfig.WebPort = value; }
        }
        
        public bool BlockIncoming
        {
            get { return Program.Instance.TcpConfig.Listen; }
            set { Program.Instance.TcpConfig.Listen = value; }
        }
        
        public bool DefaultShare
        {
            get { return Program.Instance.TcpConfig.Share; }
            set { Program.Instance.TcpConfig.Share = value; }
        }
        
        public bool ForwardQueries
        {
            get { return Program.Instance.TcpConfig.Forward; }
            set { Program.Instance.TcpConfig.Forward = value; }
        }
        
        public List<PeerInfo> Peers
        {
            get { return Program.Instance.TcpConfig.GetPeers(); }
        }

        public bool AutoStart { get; set; }

        public bool Changed
        {
            get
            {
                //TODO
                return true;
            }
        }

        public void Load()
        {
            Program.Instance.LoadConfiguration();
        }

        public void SaveChanges()
        {
            //TODO
        }

        public void Reset()
        {
            //TODO
        }
    }
}
