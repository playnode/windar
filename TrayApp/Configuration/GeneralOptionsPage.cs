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
        private bool _autostartChanged;
        private bool _nodeNameChanged;
        private bool _portChanged;
        private bool _allowIncomingChanged;
        private bool _forwardQueriesChanged;
        private bool _peersChanged;

        private bool _origAutoStart;
        private string _origNodeName;
        private int _origPort;
        private bool _origAllowIncoming;
        private bool _origForwardQueries;

        public void Load()
        {
            var mainConfig = Program.Instance.MainConfig;
            var peerConfig = Program.Instance.PeerConfig;
            _origAutoStart = false; //TODO
            _origNodeName = mainConfig.Name;
            _origPort = peerConfig.Port;
            _origAllowIncoming = peerConfig.Listen;
            _origForwardQueries = peerConfig.Forward;
        }

        public bool Changed
        {
            get
            {
                return _autostartChanged
                       || _nodeNameChanged
                       || _portChanged
                       || _allowIncomingChanged
                       || _forwardQueriesChanged
                       || _peersChanged;
            }
        }

        #region Page options

        public bool AutoStart
        {
            get
            {
                //TODO
                return false;
            }
            set
            {
                //TODO
                _autostartChanged = value != _origAutoStart;
            }
        }

        public string NodeName
        {
            get
            {
                string result;
                var configName = Program.Instance.MainConfig.Name;
                if (!string.IsNullOrEmpty(configName)) result = configName;
                else
                {
                    _origNodeName = null;
                    result = Environment.MachineName;
                }
                return result;
            }
            set
            {
                if (_origNodeName == null)
                {
                    _nodeNameChanged = (value != Environment.MachineName)
                                       && !string.IsNullOrEmpty(value);
                }
                else
                {
                    _nodeNameChanged = value != _origNodeName;
                }
                if (_nodeNameChanged)
                {
                    Program.Instance.MainConfig.Name = value;
                }
            }
        }

        public int Port
        {
            get
            {
                return Program.Instance.PeerConfig.Port;
            }
            set
            {
                _portChanged = value != _origPort;
                Program.Instance.PeerConfig.Port = value;
            }
        }
        
        public bool AllowIncoming
        {
            get
            {
                return Program.Instance.PeerConfig.Listen;
            }
            set
            {
                _allowIncomingChanged = value != _origAllowIncoming;
                Program.Instance.PeerConfig.Listen = value;
            }
        }
        
        public bool ForwardQueries
        {
            get
            {
                return Program.Instance.PeerConfig.Forward;
            }
            set
            {
                _forwardQueriesChanged = value != _origForwardQueries;
                Program.Instance.PeerConfig.Forward = value;
            }
        }

        #region Peers

        public List<PeerInfo> Peers
        {
            get
            {
                return Program.Instance.PeerConfig.GetPeers();
            }
        }

        public void RemovePeer(string host, int port)
        {
            //TODO
            _peersChanged = true;
        }

        public void AddNewPeer(string host, int port, bool share)
        {
            //TODO
            _peersChanged = true;
        }

        #endregion

        #endregion
    }
}
