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
using System.Reflection;
using System.Text;
using log4net;
using Windar.TrayApp.Configuration.Parser;

namespace Windar.TrayApp.Configuration
{
    public class MainConfigFile : ErlangTermsDocument
    {
        private NamedString _nodeName;
        private NamedBoolean _crossDomain;
        private NamedBoolean _explain;
        private NamedString _authdbdir;
        private NamedList _web;
        private NamedList _scripts;
        private NamedList _blacklist;

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

        public int HttpPort
        {
            get
            {
                var result = -1;

                if (_web == null) _web = FindNamedList("web");
                if (_web != null)
                {
                    // Extract http port from web list.
                    foreach (var token in _web.Tokens)
                    {
                        // Only interested in tuples in the list token.
                        if (!(token is ListToken)) continue;
                        foreach (var listToken in ((ListToken) token).Tokens)
                        {
                            // Only interested in tuple tokens here.
                            if (!(listToken is TupleToken)) continue;

                            // Find tuple with name "port".
                            var foundName = false;
                            foreach (var tupleToken in ((TupleToken) listToken).Tokens)
                            {
                                // Only interested in value tokens here.
                                if (!(tupleToken is IValueToken)) continue;
                                if (!foundName)
                                {
                                    if (!(tupleToken is AtomToken)) continue;
                                    if (((AtomToken) tupleToken).Text != "port") break;
                                    foundName = true;
                                    continue;
                                }
                                if (!(tupleToken is IntegerToken)) break;
                                result = Int32.Parse(((IntegerToken) tupleToken).Text);
                            }
                        }
                    }
                }
                return result;
            }
            set
            {
                if (_web == null) _web = FindNamedList("web");
                if (_web != null)
                {
                    //TODO: Exctract http port from web list and update it.
                }
                else
                {
                    //TODO: Create web list and set http port as specified.
                }
            }
        }
        
        public int Max
        {
            get
            {
                //TODO
                throw new NotImplementedException();
            }
            set
            {
                //TODO
                throw new NotImplementedException();
            }
        }

        public string ListeningIp
        {
            get
            {
                //TODO
                throw new NotImplementedException();
            }
            set
            {
                //TODO
                throw new NotImplementedException();
            }
        }

        public string DocRoot
        {
            get
            {
                //TODO
                throw new NotImplementedException();
            }
            set
            {
                //TODO
                throw new NotImplementedException();
            }
        }

        public string LibraryDbDir
        {
            get
            {
                //TODO
                throw new NotImplementedException();
            }
            set
            {
                //TODO
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Resolver scripts list management.

        public List<string> ListResolverScripts()
        {
            //TODO
            throw new NotImplementedException();
        }

        public void AddResolverScript(string path)
        {
            //TODO
            throw new NotImplementedException();
        }

        public void RemoveResolverScript(string path)
        {
            //TODO
            throw new NotImplementedException();
        }

        #endregion

        #region Modules blacklist management.

        public List<string> ListModulesBlacklist()
        {
            //TODO
            throw new NotImplementedException();
        }

        public void AddModuleToBlacklist()
        {
            //TODO
            throw new NotImplementedException();
        }

        public void RemoveModuleFromBlacklist()
        {
            //TODO
            throw new NotImplementedException();
        }

        #endregion
    }
}
