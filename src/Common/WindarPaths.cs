using System;
using System.Text;

namespace Windar.Common
{
    public class WindarPaths
    {
        string _windarProgramFiles;

        public string WindarProgramFiles
        {
            get { return _windarProgramFiles; }
            set { _windarProgramFiles = value; }
        }

        public WindarPaths(string appPath)
        {
            WindarProgramFiles = appPath;
        }

        public string LocalPlaydarUrl
        {
            get
            {
                //TODO: Get the configured URL from config.
                return "http://127.0.0.1:60210/";
            }
        }

        string _erlPath;
        public string ErlPath
        {
            get
            {
                return _erlPath ?? 
                    (_erlPath = WindarProgramFiles + @"\minimerl");
            }
        }

        string _erlCmd;
        public string ErlCmd
        {
            get
            {
                return _erlCmd ?? 
                    (_erlCmd = ErlPath + @"\bin\erl.exe");
            }
        }

        string _playdarPath;
        public string PlaydarProgramPath
        {
            get
            {
                return _playdarPath ?? 
                    (_playdarPath = WindarProgramFiles + @"\playdar");
            }
        }

        string _playdarEtcPath;
        public string PlaydarEtcPath
        {
            get
            {
                return _playdarEtcPath ??
                    (_playdarEtcPath = WindarAppData + @"\etc");
            }
        }

        string _playdarDataPath;
        public string WindarAppData
        {
            get
            {
                if (_playdarDataPath == null)
                {
                    StringBuilder str = new StringBuilder();
                    str.Append(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                    str.Append(@"\Windar");
                    _playdarDataPath = str.ToString();
                }
                return _playdarDataPath;
            }
        }

        #region Conversion methods.

        public static string ToWindowsPath(string path)
        {
            return path.Replace('/', '\\');
        }

        public static string ToUnixPath(string path)
        {
            return path.Replace('\\', '/');
        }

        #endregion
    }
}
