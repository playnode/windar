namespace Windar.PlayerPlugin
{
    class PlayItem
    {
        string _sId;
        string _artist;
        string _track;
        string _album;
        string _mimeType;
        float _score;
        int _duration;
        int _bitrate;
        int _size;
        string _source;

        public string SId
        {
            get { return _sId; }
            set { _sId = value; }
        }

        public string Artist
        {
            get { return _artist; }
            set { _artist = value; }
        }

        public string Track
        {
            get { return _track; }
            set { _track = value; }
        }

        public string Album
        {
            get { return _album; }
            set { _album = value; }
        }

        public string MimeType
        {
            get { return _mimeType; }
            set { _mimeType = value; }
        }

        public float Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public int Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        public int Bitrate
        {
            get { return _bitrate; }
            set { _bitrate = value; }
        }

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }
    }
}
