using System;
using System.Collections.Generic;

namespace Windar.TrayApp.Configuration
{
    class GeneralOptionsPage : IOptionsPage
    {
        // Change flags.
        bool _autostartChanged;
        bool _nodeNameChanged;
        bool _portChanged;
        bool _allowIncomingChanged;
        bool _forwardQueriesChanged;

        // Original values.
        bool _origAutoStart;
        string _origNodeName;
        int _origPort;
        bool _origAllowIncoming;
        bool _origForwardQueries;

        // Changed list items handled differently.
        // Not checking each and every value here.
        bool _peersChanged;

        bool _newPeersToAdd;
        bool _peerValueChanged;

        public bool NewPeersToAdd
        {
            get { return _newPeersToAdd; }
            set { _newPeersToAdd = value; }
        }

        public bool PeerValueChanged
        {
            get { return _peerValueChanged; }
            set { _peerValueChanged = value; }
        }

        public void Load()
        {
            MainConfigFile mainConfig = Program.Instance.Config.MainConfig;
            TcpConfigFile peerConfig = Program.Instance.Config.PeersConfig;
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
                string configName = Program.Instance.Config.MainConfig.Name;
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
                    Program.Instance.Config.MainConfig.Name = value;
                }
            }
        }

        public int Port
        {
            get
            {
                return Program.Instance.Config.PeersConfig.Port;
            }
            set
            {
                _portChanged = value != _origPort;
                Program.Instance.Config.PeersConfig.Port = value;
            }
        }
        
        public bool AllowIncoming
        {
            get
            {
                return Program.Instance.Config.PeersConfig.Listen;
            }
            set
            {
                _allowIncomingChanged = value != _origAllowIncoming;
                Program.Instance.Config.PeersConfig.Listen = value;
            }
        }
        
        public bool ForwardQueries
        {
            get
            {
                return Program.Instance.Config.PeersConfig.Forward;
            }
            set
            {
                _forwardQueriesChanged = value != _origForwardQueries;
                Program.Instance.Config.PeersConfig.Forward = value;
            }
        }

        #region Peers

        public List<PeerInfo> Peers
        {
            get
            {
                return Program.Instance.Config.PeersConfig.GetPeers();
            }
        }

        public void RemovePeer(string host, int port)
        {
            Program.Instance.Config.PeersConfig.RemovePeer(host, port);
            _peersChanged = true;
        }

        public void AddNewPeer(string host, int port, bool share)
        {
            Program.Instance.Config.PeersConfig.SetPeerInfo(host, port, share);
            _peersChanged = true;
        }

        #endregion

        #endregion
    }
}
