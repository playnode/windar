using System.Collections.Generic;

namespace Windar.TrayApp.Configuration
{
    class LibraryOptionsPage : IOptionsPage
    {
        List<string> _scanPaths;
        bool _scanPathsChanged;

        int _savedScanPathCount;
        bool _scanPathValueChanged;
        bool _scanPathsToAdd;
        bool _scanPathsRemoved;

        public int SavedScanPathCount
        {
            get { return _savedScanPathCount; }
            set { _savedScanPathCount = value; }
        }

        public bool ScanPathValueChanged
        {
            get { return _scanPathValueChanged; }
            set { _scanPathValueChanged = value; }
        }

        public bool ScanPathsToAdd
        {
            get { return _scanPathsToAdd; }
            set { _scanPathsToAdd = value; }
        }

        public bool ScanPathsRemoved
        {
            get { return _scanPathsRemoved; }
            set { _scanPathsRemoved = value; }
        }

        public void Load()
        {
            SavedScanPathCount = ScanPaths.Count;
        }

        public bool Changed
        {
            get
            {
                return _scanPathsChanged
                       || ScanPathValueChanged
                       || ScanPathsToAdd;
            }
        }

        #region Page options

        #region Scan paths

        public List<string> ScanPaths
        {
            get
            {
                if (_scanPaths == null) 
                    _scanPaths = Program.Instance.Config.MainConfig.ListScanPaths();
                return _scanPaths;
            }
        }

        public void RemoveScanPath(string path)
        {
            Program.Instance.Config.MainConfig.RemoveScanPath(path);
            ScanPathsRemoved = true;
            _scanPaths = Program.Instance.Config.MainConfig.ListScanPaths();
            _scanPathsChanged = true;
        }

        public void AddScanPath(string path)
        {
            Program.Instance.Config.MainConfig.AddScanPath(path);
            _scanPaths = Program.Instance.Config.MainConfig.ListScanPaths();
            _scanPathsChanged = true;
        }

        #endregion

        #endregion
    }
}
