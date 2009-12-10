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
using System.IO;
using System.Reflection;
using System.Text;
using log4net;
using Windar.TrayApp.Configuration.Parser;
using Windar.TrayApp.Configuration.Parser.Tokens;
using Windar.TrayApp.Configuration.Values;

namespace Windar.TrayApp.Configuration
{
    public class MainConfigFile : IConfigFile
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        private ErlangTermsDocument _configFile;
        private NamedString _nodeName;
        private NamedBoolean _crossDomain;

        #region Properties

        private static string Timestamp
        {
            get
            {
                var result = new StringBuilder();
                result.Append(DateTime.Now.ToShortTimeString());
                result.Append(", ").Append(DateTime.Now.ToLongDateString());
                return result.ToString();
            }
        }

        public string NodeName
        {
            get
            {
                if (Log.IsDebugEnabled) Log.Debug("Getting NodeName");
                string result = null;
                if (_nodeName == null)
                {
                    if (Log.IsDebugEnabled) Log.Debug("Finding NodeName");
                    _nodeName = _configFile.FindNamedString("name");
                    if (Log.IsDebugEnabled) Log.Debug("Found NodeName");
                }
                if (_nodeName != null)
                {
                    if (Log.IsDebugEnabled) Log.Debug("Returning NodeName.Value");
                    result = _nodeName.Value;
                }
                return result;
            }
            set
            {
                if (_nodeName == null) _nodeName = _configFile.FindNamedString("name");
                if (_nodeName != null) _nodeName.Value = value;
                else
                {
                    _nodeName = new NamedString("name", value);
                    _configFile.Tokens.Add(new WhitespaceToken { Text = "\n\n" });
                    _configFile.Tokens.Add(new CommentToken { Text = "% Added by Windar " + Timestamp });
                    _configFile.Tokens.Add(_nodeName);
                }
            }
        }

        public bool CrossDomain
        {
            get
            {
                var result = false;
                if (_crossDomain == null) _crossDomain = _configFile.FindNamedBoolean("crossdomain");
                if (_crossDomain != null) result = _crossDomain.Value;
                return result;
            }
            set
            {
                if (_crossDomain == null) _crossDomain = _configFile.FindNamedBoolean("crossdomain");
                if (_crossDomain != null) _crossDomain.Value = value;
                else
                {
                    _crossDomain = new NamedBoolean("crossdomain", value);
                    _configFile.Tokens.Add(new WhitespaceToken { Text = "\n\n" });
                    _configFile.Tokens.Add(new CommentToken { Text = "% Added by Windar " + Timestamp });
                    _configFile.Tokens.Add(_nodeName);
                }
            }
        }

        public List<string> Scripts { get; private set; }
        public int HttpPort { get; set; }
        public int Max { get; set; } //TODO: Max what?
        public string ListeningIp { get; set; }
        public string DocRoot { get; set; }
        public List<string> ModulesBlacklist { get; private set; }
        public bool ExplainResults { get; set; }
        public string LibraryDbPath { get; set; }
        public string AuthDbPath { get; set; }

        #endregion

        public MainConfigFile()
        {
            //Scripts = new List<string>();
            //ModulesBlacklist = new List<string>();
        }

        public void Load(string filename)
        {
            _configFile = new ErlangTermsDocument();
            _configFile.Load(new FileInfo(filename));
        }

        public override string ToString()
        {
            return _configFile.ToString();
        }

        public void Save()
        {
            _configFile.Save();
        }
    }
}
