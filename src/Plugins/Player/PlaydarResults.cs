using System.Collections.Generic;

namespace Windar.PlayerPlugin
{
    class PlaydarResults
    {
        int _pollInterval;
        int _pollLimit;
        bool _solved;
        List<PlayItem> _playItems;

        public int PollInterval
        {
            get { return _pollInterval; }
            set { _pollInterval = value; }
        }

        public int PollLimit
        {
            get { return _pollLimit; }
            set { _pollLimit = value; }
        }

        public bool Solved
        {
            get { return _solved; }
            set { _solved = value; }
        }

        public List<PlayItem> PlayItems
        {
            get { return _playItems; }
            set { _playItems = value; }
        }

        public PlaydarResults()
        {
            PlayItems = new List<PlayItem>();
        }
    }
}
