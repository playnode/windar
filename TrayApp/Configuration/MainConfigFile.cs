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
using System.Text;
using Windar.TrayApp.Configuration.Parser;
using Windar.TrayApp.Configuration.Parser.Tokens;

namespace Windar.TrayApp.Configuration
{
    public class MainConfigFile : IConfigFile
    {
        private ErlangTermsDocument _configFile;
        private SingleValueAtomTuple _nodeNameTuple;

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
                string result = null;
                if (_nodeNameTuple == null) _nodeNameTuple = _configFile.FindSingleValueAtomTuple("name");
                if (_nodeNameTuple != null) result = _nodeNameTuple.Name;
                return result;
            }
            set
            {
                if (_nodeNameTuple == null) _nodeNameTuple = _configFile.FindSingleValueAtomTuple("name");
                if (_nodeNameTuple != null) _nodeNameTuple.Value = new StringToken { Text = value };
                else
                {
                    _nodeNameTuple = new SingleValueAtomTuple("name", value);
                    _configFile.Tokens.Add(new WhitespaceToken { Text = "\n\n" });
                    _configFile.Tokens.Add(new CommentToken { Text = "% Added by Windar " + Timestamp });
                    _configFile.Tokens.Add(_nodeNameTuple);
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
            Scripts = new List<string>();
            ModulesBlacklist = new List<string>();
        }

        public void Load(string filename)
        {
            _configFile = new ErlangTermsDocument();
            _configFile.Load(new FileInfo(filename));

            // Extract data from file, for defined properties.
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
