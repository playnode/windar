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
using log4net;
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
                    Tokens.Add(new WhitespaceToken("\n\n"));
                    Tokens.Add(new WindarAddedComment());
                    Tokens.Add(_nodeName);
                    Tokens.Add(new TermEndToken());
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
                    Tokens.Add(new WhitespaceToken("\n\n"));
                    Tokens.Add(new WindarAddedComment());
                    Tokens.Add(_authdbdir);
                    Tokens.Add(new TermEndToken());
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
                    Tokens.Add(new WhitespaceToken("\n\n"));
                    Tokens.Add(new WindarAddedComment());
                    Tokens.Add(_crossDomain);
                    Tokens.Add(new TermEndToken());
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
                    Tokens.Add(new WhitespaceToken("\n\n"));
                    Tokens.Add(new WindarAddedComment());
                    Tokens.Add(_explain);
                    Tokens.Add(new TermEndToken());
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

            Tokens.Add(new WhitespaceToken("\n\n"));
            Tokens.Add(new WindarAddedComment());
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
            Tokens.Add(result);
            Tokens.Add(new TermEndToken());
            return result;
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
                Tokens.Add(new WhitespaceToken("\n\n"));
                Tokens.Add(new WindarAddedComment());
                Tokens.Add(_scripts);
                Tokens.Add(new TermEndToken());
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
            var result = new List<string>();
            if (_blacklist == null) _blacklist = FindNamedList("modules_blacklist");
            if (_blacklist != null)
            {
                foreach (var token in _blacklist.Tokens)
                {
                    if (!(token is StringToken)) continue;
                    result.Add(((StringToken) token).Text);
                }
            }
            return result;
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
