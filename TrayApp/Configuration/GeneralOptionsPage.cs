/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009 Steven Robertson <steve@playnode.org>
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
using System.Collections.Generic;

namespace Windar.TrayApp.Configuration
{
    class GeneralOptionsPage : IOptionsPage
    {
        // Change flags.
        private bool _autostartChanged;
        private bool _nodeNameChanged;
        private bool _portChanged;
        private bool _allowIncomingChanged;
        private bool _forwardQueriesChanged;

        // Original values.
        private bool _origAutoStart;
        private string _origNodeName;
        private int _origPort;
        private bool _origAllowIncoming;
        private bool _origForwardQueries;

        // Changed list items handled differently.
        // Not checking each and every value here.
        private bool _peersChanged;

        public bool NewPeersToAdd { get; set; }
        public bool PeerValueChanged { get; set; }

        public void Load()
        {
            var mainConfig = Program.Instance.Config.Main;
            var peerConfig = Program.Instance.Config.Peers;
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
                       || _peersChanged
                       || NewPeersToAdd
                       || PeerValueChanged;
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
                var configName = Program.Instance.Config.Main.Name;
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
                    Program.Instance.Config.Main.Name = value;
                }
            }
        }

        public int Port
        {
            get
            {
                return Program.Instance.Config.Peers.Port;
            }
            set
            {
                _portChanged = value != _origPort;
                Program.Instance.Config.Peers.Port = value;
            }
        }
        
        public bool AllowIncoming
        {
            get
            {
                return Program.Instance.Config.Peers.Listen;
            }
            set
            {
                _allowIncomingChanged = value != _origAllowIncoming;
                Program.Instance.Config.Peers.Listen = value;
            }
        }
        
        public bool ForwardQueries
        {
            get
            {
                return Program.Instance.Config.Peers.Forward;
            }
            set
            {
                _forwardQueriesChanged = value != _origForwardQueries;
                Program.Instance.Config.Peers.Forward = value;
            }
        }

        #region Peers

        public List<PeerInfo> Peers
        {
            get
            {
                return Program.Instance.Config.Peers.GetPeers();
            }
        }

        public void RemovePeer(string host, int port)
        {
            Program.Instance.Config.Peers.RemovePeer(host, port);
            _peersChanged = true;
        }

        public void AddNewPeer(string host, int port, bool share)
        {
            Program.Instance.Config.Peers.SetPeerInfo(host, port, share);
            _peersChanged = true;
        }

        #endregion

        #endregion
    }
}
