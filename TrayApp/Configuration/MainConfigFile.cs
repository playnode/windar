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

using System.Collections.Generic;
using Windar.TrayApp.Configuration.Parser;

namespace Windar.TrayApp.Configuration
{
    public class MainConfigFile : ErlangTermsDocument
    {
        private NamedString _nodeName;
        private NamedBoolean _crossDomain;
        private NamedBoolean _explain;
        private NamedString _authdbdir;
        private TupleToken _libdbdir;
        private NamedList _web;
        private NamedList _scripts;
        private NamedList _blacklist;

        #region Properties

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
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new WindarAddedComment());
                    Document.Tokens.Add(_nodeName);
                    Document.Tokens.Add(new TermEndToken());
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
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new WindarAddedComment());
                    Document.Tokens.Add(_authdbdir);
                    Document.Tokens.Add(new TermEndToken());
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
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new WindarAddedComment());
                    Document.Tokens.Add(_crossDomain);
                    Document.Tokens.Add(new TermEndToken());
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
                    Document.Tokens.Add(new WhitespaceToken("\n\n"));
                    Document.Tokens.Add(new WindarAddedComment());
                    Document.Tokens.Add(_explain);
                    Document.Tokens.Add(new TermEndToken());
                }
            }
        }

        public int HttpPort
        {
            get
            {
                var result = -1;
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

        public int Max
        {
            get
            {
                var result = -1;
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

        public string ListeningIp
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

        public string DocRoot
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

        public string LibraryDbDir
        {
            get
            {
                string result = null;
                if (_libdbdir == null) _libdbdir = FindLibraryDbDir();
                if (_libdbdir != null)
                {
                    var valueTokens = _libdbdir.GetValueTokens();
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
                    Document.Tokens.Add(new WindarAddedComment());
                    _libdbdir = new TupleToken();
                    var tuple = new TupleToken();
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
                var valueTokens = _libdbdir.GetValueTokens();
                ((StringToken) valueTokens[1]).Text = value;
            }
        }

        #endregion

        private NamedList CreateWebConfigItem()
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
            Document.Tokens.Add(new WindarAddedComment());
            var list = new ListToken();

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

            var result = new NamedList("web", list);
            Document.Tokens.Add(result);
            Document.Tokens.Add(new TermEndToken());
            return result;
        }

        private TupleToken FindLibraryDbDir()
        {
            TupleToken result = null;

            // Search through all the tuples at the top-level of file.
            foreach (var token in Document.GetTupleTokens())
            {
                // Only interested in a tuple with a tuple as a first item.
                var valueTokens = token.GetValueTokens();
                if (!(valueTokens[0] is TupleToken)) continue;

                // Check the tuple names expected: {library, dbdir}
                var tuple = (TupleToken) valueTokens[0];
                var values = tuple.GetValueTokens();
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

        #region Resolver scripts list management.

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
                Document.Tokens.Add(new WindarAddedComment());
                Document.Tokens.Add(_scripts);
                Document.Tokens.Add(new TermEndToken());
            }
            _scripts.AddListItem(script);
        }

        public void RemoveScript(string script)
        {
            if (_scripts == null) _scripts = FindNamedList("scripts");
            if (_scripts != null) _scripts.RemoveListItem(script);
        }

        #endregion

        #region Modules blacklist management.

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
                Document.Tokens.Add(new WindarAddedComment());
                Document.Tokens.Add(_blacklist);
                Document.Tokens.Add(new TermEndToken());
            }
            _blacklist.AddListItem(module);
        }

        public void UnblockModule(string module)
        {
            if (_blacklist == null) _blacklist = FindNamedList("modules_blacklist");
            if (_blacklist != null) _blacklist.RemoveListItem(module);
        }

        #endregion
    }
}
