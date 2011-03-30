/*
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010, 2011 Steven Robertson <steve@playnode.com>
 *
 * Windar - Playdar for Windows
 *
 * Windar is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License (LGPL) as published
 * by the Free Software Foundation; either version 2.1 of the License, or (at
 * your option) any later version.
 *
 * Windar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License version 2.1 for more details
 * (a copy is included in the LICENSE file that accompanied this code).
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using Playnode.ErlangTerms.Parser;
using Windar.Common;

namespace Windar.TrayApp.Configuration
{
    public class MainConfigFile : ErlangTermsDocument
    {
        NamedString _name;
        NamedList _scripts;
        NamedList _web;
        NamedBoolean _crossdomain;
        NamedList _blacklist;
        NamedString _authdbdir;
        NamedBoolean _explain;
        TupleToken _libdbdir;
        NamedList _scanpaths;
        TupleToken _asUsername;
        TupleToken _asPassword;

        public string Name
        {
            get
            {
                string result = null;
                if (_name == null) _name = FindNamedString("name");
                if (_name != null) result = _name.Value;
                return result;
            }
            set
            {
                if (_name == null) _name = FindNamedString("name");
                if (_name != null) _name.Value = value;
                else
                {
                    _name = new NamedString("name", value);
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new AddedComment());
                    Document.Tokens.Add(_name);
                    Document.Tokens.Add(new TermEndToken());
                }
            }
        }

        #region Scripts

        public List<string> ListScripts()
        {
            List<string> result = null;
            if (_scripts == null) _scripts = FindNamedList("scripts");
            if (_scripts != null) result = _scripts.GetStringsList();
            return result;
        }

        public void AddScript(string script)
        {
            if (_scripts == null) _scripts = FindNamedList("scripts");
            if (_scripts == null)
            {
                _scripts = new NamedList("scripts", new ListToken());
                Document.Tokens.Add(new WhitespaceToken("\n\n"));
                Document.Tokens.Add(new AddedComment());
                Document.Tokens.Add(_scripts);
                Document.Tokens.Add(new TermEndToken());
            }
            _scripts.AddStringsListItem(script);
        }

        public void RemoveScript(string script)
        {
            if (_scripts == null) _scripts = FindNamedList("scripts");
            if (_scripts != null) _scripts.RemoveStringsListItem(script);
        }

        #endregion

        #region Web

        NamedList CreateWebConfigItem()
        {
            /*
            {web,[
                {port, 60210},
                {max, 100},
                {ip, "0.0.0.0"}, 
                {docroot, "priv/www"}
            ]}.
            */

            Document.Tokens.Add(new WhitespaceToken("\n\n"));
            Document.Tokens.Add(new AddedComment());
            ListToken list = new ListToken();

            // Port
            list.Tokens.Add(new WhitespaceToken("\n    "));
            list.Tokens.Add(new NamedInteger("port", 60210));
            list.Tokens.Add(new CommaToken());

            // Max
            list.Tokens.Add(new WhitespaceToken("\n    "));
            list.Tokens.Add(new NamedInteger("max", 100));
            list.Tokens.Add(new CommaToken());

            // IP
            list.Tokens.Add(new WhitespaceToken("\n    "));
            list.Tokens.Add(new NamedString("ip", "0.0.0.0"));
            list.Tokens.Add(new CommaToken());

            // DocRoot
            list.Tokens.Add(new WhitespaceToken("\n    "));
            list.Tokens.Add(new NamedString("docroot", "priv/www"));
            list.Tokens.Add(new WhitespaceToken("\n"));

            NamedList result = new NamedList("web", list);
            Document.Tokens.Add(result);
            Document.Tokens.Add(new TermEndToken());
            return result;
        }

        public int WebPort
        {
            get
            {
                int result = -1;
                if (_web == null) _web = FindNamedList("web");
                if (_web != null) result = _web.GetNamedInteger("port");
                return result;
            }
            set
            {
                if (_web == null) _web = FindNamedList("web");
                if (_web == null) _web = CreateWebConfigItem();
                _web.SetNamedValue("port", value);
            }
        }

        public int WebMax
        {
            get
            {
                int result = -1;
                if (_web == null) _web = FindNamedList("web");
                if (_web != null) result = _web.GetNamedInteger("max");
                return result;
            }
            set
            {
                if (_web == null) _web = FindNamedList("web");
                if (_web == null) _web = CreateWebConfigItem();
                _web.SetNamedValue("max", value);
            }
        }

        public string WebIp
        {
            get
            {
                string result = null;
                if (_web == null) _web = FindNamedList("web");
                if (_web != null) result = _web.GetNamedString("ip");
                return result;
            }
            set
            {
                if (_web == null) _web = FindNamedList("web");
                if (_web == null) _web = CreateWebConfigItem();
                _web.SetNamedValue("ip", value);
            }
        }

        public string WebDocRoot
        {
            get
            {
                string result = null;
                if (_web == null) _web = FindNamedList("web");
                if (_web != null) result = _web.GetNamedString("docroot");
                return result;
            }
            set
            {
                if (_web == null) _web = FindNamedList("web");
                if (_web == null) _web = CreateWebConfigItem();
                _web.SetNamedValue("docroot", value);
            }
        }

        #endregion

        public bool CrossDomain
        {
            get
            {
                bool result = false;
                if (_crossdomain == null) _crossdomain = FindNamedBoolean("crossdomain");
                if (_crossdomain != null) result = _crossdomain.Value;
                return result;
            }
            set
            {
                if (_crossdomain == null) _crossdomain = FindNamedBoolean("crossdomain");
                if (_crossdomain != null) _crossdomain.Value = value;
                else
                {
                    _crossdomain = new NamedBoolean("crossdomain", value);
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new AddedComment());
                    Document.Tokens.Add(_crossdomain);
                    Document.Tokens.Add(new TermEndToken());
                }
            }
        }

        #region Modules blacklist

        public List<string> ListModulesBlacklist()
        {
            List<string> result = null;
            if (_blacklist == null) _blacklist = FindNamedList("modules_blacklist");
            if (_blacklist != null) result = _blacklist.GetStringsList();
            return result;
        }

        public void BlockModule(string module)
        {
            if (_blacklist == null) _blacklist = FindNamedList("modules_blacklist");
            if (_blacklist == null)
            {
                _blacklist = new NamedList("modules_blacklist", new ListToken());
                Document.Tokens.Add(new WhitespaceToken("\n\n"));
                Document.Tokens.Add(new AddedComment());
                Document.Tokens.Add(_blacklist);
                Document.Tokens.Add(new TermEndToken());
            }
            _blacklist.AddStringsListItem(module);
        }

        public void UnblockModule(string module)
        {
            if (_blacklist == null) _blacklist = FindNamedList("modules_blacklist");
            if (_blacklist != null) _blacklist.RemoveStringsListItem(module);
        }

        #endregion

        public bool Explain
        {
            get
            {
                bool result = false;
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
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new AddedComment());
                    Document.Tokens.Add(_explain);
                    Document.Tokens.Add(new TermEndToken());
                }
            }
        }

        public string LibraryDbDir
        {
            get
            {
                string result = null;
                if (_libdbdir == null) _libdbdir = FindLibraryDbDir();
                if (_libdbdir != null)
                {
                    List<IValueToken> valueTokens = _libdbdir.GetValueTokens();
                    result = ((StringToken) valueTokens[1]).Text;
                }
                return result;
            }
            set
            {
                if (_libdbdir == null) _libdbdir = FindLibraryDbDir();
                if (_libdbdir == null)
                {
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new AddedComment());
                    _libdbdir = new TupleToken();
                    TupleToken tuple = new TupleToken();
                    tuple.Tokens.Add(new AtomToken("library"));
                    tuple.Tokens.Add(new CommaToken());
                    tuple.Tokens.Add(new WhitespaceToken(" "));
                    tuple.Tokens.Add(new AtomToken("dbdir"));
                    _libdbdir.Tokens.Add(tuple);
                    _libdbdir.Tokens.Add(new CommaToken());
                    _libdbdir.Tokens.Add(new WhitespaceToken(" "));
                    _libdbdir.Tokens.Add(new StringToken(value));
                    Document.Tokens.Add(_libdbdir);
                    Document.Tokens.Add(new TermEndToken());
                }
                List<IValueToken> valueTokens = _libdbdir.GetValueTokens();
                ((StringToken) valueTokens[1]).Text = value;
            }
        }

        TupleToken FindLibraryDbDir()
        {
            TupleToken result = null;

            // Search through all the tuples at the top-level of file.
            foreach (TupleToken token in Document.GetTupleTokens())
            {
                // Only interested in a tuple with a tuple as a first item.
                List<IValueToken> valueTokens = token.GetValueTokens();
                if (!(valueTokens[0] is TupleToken)) continue;

                // Check the tuple names expected: {library, dbdir}
                TupleToken tuple = (TupleToken)valueTokens[0];
                List<IValueToken> values = tuple.GetValueTokens();
                if (!(values[0] is AtomToken) || ((AtomToken) values[0]).Text != "library" ||
                    !(values[1] is AtomToken) || ((AtomToken) values[1]).Text != "dbdir") continue;

                // Ensure it has the expected StringToken type.
                if (valueTokens[1] is StringToken)
                {
                    // Found result.
                    result = token;
                }
            }
            return result;
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
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new AddedComment());
                    Document.Tokens.Add(_authdbdir);
                    Document.Tokens.Add(new TermEndToken());
                }
            }
        }

        #region Scan paths

        public List<string> ListScanPaths()
        {
            List<string> result = null;
            if (_scanpaths == null) _scanpaths = FindNamedList("scan_paths");
            if (_scanpaths != null) result = _scanpaths.GetStringsList();
            if (result == null) result = new List<string>();
            return result;
        }

        public void AddScanPath(string path)
        {
            if (_scanpaths == null) _scanpaths = FindNamedList("scan_paths");
            if (_scanpaths == null)
            {
                _scanpaths = new NamedList("scan_paths", new ListToken());
                Document.Tokens.Add(new WhitespaceToken("\n\n"));
                Document.Tokens.Add(new AddedComment());
                Document.Tokens.Add(_scanpaths);
                Document.Tokens.Add(new TermEndToken());
            }
            _scanpaths.AddStringsListItem(path);
        }

        public void RemoveScanPath(string path)
        {
            if (_scanpaths == null) _scanpaths = FindNamedList("scan_paths");
            if (_scanpaths != null) _scanpaths.RemoveStringsListItem(path);
        }

        #endregion

        #region Scrobbler credentials

        public Credentials ScrobblerCredentials
        {
            get
            {
                return new Credentials(AudioScrobblerUsername, AudioScrobblerPassword);
            }
            set
            {
                AudioScrobblerUsername = value.Username;
                AudioScrobblerPassword = value.Password;
            }
        }

        public string AudioScrobblerUsername
        {
            get
            {
                string result = null;
                if (_asUsername == null) _asUsername = FindAudioScrobblerUsername();
                if (_asUsername != null)
                {
                    List<IValueToken> valueTokens = _asUsername.GetValueTokens();
                    result = ((StringToken) valueTokens[1]).Text;
                }
                return result;
            }
            set
            {
                if (_asUsername == null) _asUsername = FindAudioScrobblerUsername();
                if (_asUsername == null)
                {
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new AddedComment());
                    _asUsername = new TupleToken();
                    TupleToken tuple = new TupleToken();
                    tuple.Tokens.Add(new AtomToken("as"));
                    tuple.Tokens.Add(new CommaToken());
                    tuple.Tokens.Add(new WhitespaceToken(" "));
                    tuple.Tokens.Add(new AtomToken("username"));
                    _asUsername.Tokens.Add(tuple);
                    _asUsername.Tokens.Add(new CommaToken());
                    _asUsername.Tokens.Add(new WhitespaceToken(" "));
                    _asUsername.Tokens.Add(new StringToken(value));
                    Document.Tokens.Add(_asUsername);
                    Document.Tokens.Add(new TermEndToken());
                }
                List<IValueToken> valueTokens = _asUsername.GetValueTokens();
                ((StringToken) valueTokens[1]).Text = value;
            }
        }

        TupleToken FindAudioScrobblerUsername()
        {
            TupleToken result = null;

            // Search through all the tuples at the top-level of file.
            foreach (TupleToken token in Document.GetTupleTokens())
            {
                // Only interested in a tuple with a tuple as a first item.
                List<IValueToken> valueTokens = token.GetValueTokens();
                if (!(valueTokens[0] is TupleToken)) continue;

                // Check the tuple names expected: {library, dbdir}
                TupleToken tuple = (TupleToken)valueTokens[0];
                List<IValueToken> values = tuple.GetValueTokens();
                if (!(values[0] is AtomToken) || ((AtomToken) values[0]).Text != "as" ||
                    !(values[1] is AtomToken) || ((AtomToken) values[1]).Text != "username") continue;

                // Ensure it has the expected StringToken type.
                if (valueTokens[1] is StringToken)
                {
                    // Found result.
                    result = token;
                }
            }
            return result;
        }

        public string AudioScrobblerPassword
        {
            get
            {
                string result = null;
                if (_asPassword == null) _asPassword = FindAudioScrobblerPassword();
                if (_asPassword != null)
                {
                    List<IValueToken> valueTokens = _asPassword.GetValueTokens();
                    result = ((StringToken) valueTokens[1]).Text;
                }
                return result;
            }
            set
            {
                if (_asPassword == null) _asPassword = FindAudioScrobblerPassword();
                if (_asPassword == null)
                {
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new AddedComment());
                    _asPassword = new TupleToken();
                    TupleToken tuple = new TupleToken();
                    tuple.Tokens.Add(new AtomToken("as"));
                    tuple.Tokens.Add(new CommaToken());
                    tuple.Tokens.Add(new WhitespaceToken(" "));
                    tuple.Tokens.Add(new AtomToken("password"));
                    _asPassword.Tokens.Add(tuple);
                    _asPassword.Tokens.Add(new CommaToken());
                    _asPassword.Tokens.Add(new WhitespaceToken(" "));
                    _asPassword.Tokens.Add(new StringToken(value));
                    Document.Tokens.Add(_asPassword);
                    Document.Tokens.Add(new TermEndToken());
                }
                List<IValueToken> valueTokens = _asPassword.GetValueTokens();
                ((StringToken) valueTokens[1]).Text = value;
            }
        }

        TupleToken FindAudioScrobblerPassword()
        {
            TupleToken result = null;

            // Search through all the tuples at the top-level of file.
            foreach (TupleToken token in Document.GetTupleTokens())
            {
                // Only interested in a tuple with a tuple as a first item.
                List<IValueToken> valueTokens = token.GetValueTokens();
                if (!(valueTokens[0] is TupleToken)) continue;

                // Check the tuple names expected: {library, dbdir}
                TupleToken tuple = (TupleToken) valueTokens[0];
                List<IValueToken> values = tuple.GetValueTokens();
                if (!(values[0] is AtomToken) || ((AtomToken) values[0]).Text != "as" ||
                    !(values[1] is AtomToken) || ((AtomToken) values[1]).Text != "password") continue;

                // Ensure it has the expected StringToken type.
                if (valueTokens[1] is StringToken)
                {
                    // Found result.
                    result = token;
                }
            }
            return result;
        }

        #endregion
    }
}
