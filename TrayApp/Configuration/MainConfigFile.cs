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
using System.Reflection;
using System.Text;
using log4net;
using Windar.TrayApp.Configuration.Parser;
using Windar.TrayApp.Configuration.Parser;
using Windar.TrayApp.Configuration.Parser;

namespace Windar.TrayApp.Configuration
{
    public class MainConfigFile : ErlangTermsDocument
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        private NamedString _nodeName;
        private NamedBoolean _crossDomain;
        private NamedBoolean _explain;
        private NamedString _authdbdir;

        #region Properties

        private static string Timestamp
        {
            get
            {
                var result = new StringBuilder();
                result.Append(DateTime.Now.ToShortTimeString()).Append(", ");
                result.Append(DateTime.Now.ToLongDateString()).Append('.');
                return result.ToString();
            }
        }

        public string NodeName
        {
            get
            {
                string result = null;
                if (_nodeName == null) _nodeName = FindNamedString("name");
                if (_nodeName != null) result = _nodeName.Value;
                return result;
            }
            set
            {
                if (_nodeName == null) _nodeName = FindNamedString("name");
                if (_nodeName != null) _nodeName.Value = value;
                else
                {
                    _nodeName = new NamedString("name", value);
                    Tokens.Add(new WhitespaceToken { Text = "\n\n" });
                    Tokens.Add(new CommentToken { Text = "% Added by Windar " + Timestamp });
                    Tokens.Add(_nodeName);
                }
            }
        }

        public string AuthDbDir
        {
            get
            {
                string result = null;
                if (_authdbdir == null) _authdbdir = FindNamedString("authdbdir");
                if (_authdbdir != null) result = _authdbdir.Value;
                return result;
            }
            set
            {
                if (_authdbdir == null) _authdbdir = FindNamedString("authdbdir");
                if (_authdbdir != null) _authdbdir.Value = value;
                else
                {
                    _authdbdir = new NamedString("authdbdir", value);
                    Tokens.Add(new WhitespaceToken { Text = "\n\n" });
                    Tokens.Add(new CommentToken { Text = "% Added by Windar " + Timestamp });
                    Tokens.Add(_authdbdir);
                }
            }
        }

        public bool CrossDomain
        {
            get
            {
                var result = false;
                if (_crossDomain == null) _crossDomain = FindNamedBoolean("crossdomain");
                if (_crossDomain != null) result = _crossDomain.Value;
                return result;
            }
            set
            {
                if (_crossDomain == null) _crossDomain = FindNamedBoolean("crossdomain");
                if (_crossDomain != null) _crossDomain.Value = value;
                else
                {
                    _crossDomain = new NamedBoolean("crossdomain", value);
                    Tokens.Add(new WhitespaceToken { Text = "\n\n" });
                    Tokens.Add(new CommentToken { Text = "% Added by Windar " + Timestamp });
                    Tokens.Add(_crossDomain);
                }
            }
        }

        public bool Explain
        {
            get
            {
                var result = false;
                if (_explain == null) _explain = FindNamedBoolean("explain");
                if (_explain != null) result = _explain.Value;
                return result;
            }
            set
            {
                if (_explain == null) _explain = FindNamedBoolean("explain");
                if (_explain != null) _explain.Value = value;
                else
                {
                    _explain = new NamedBoolean("explain", value);
                    Tokens.Add(new WhitespaceToken { Text = "\n\n" });
                    Tokens.Add(new CommentToken { Text = "% Added by Windar " + Timestamp });
                    Tokens.Add(_explain);
                }
            }
        }

        //public List<string> Scripts { get; private set; }
        //public int HttpPort { get; set; }
        //public int Max { get; set; }
        //public string ListeningIp { get; set; }
        //public string DocRoot { get; set; }
        //public List<string> ModulesBlacklist { get; private set; }
        //public string LibraryDbDir { get; set; }

        #endregion
    }
}
