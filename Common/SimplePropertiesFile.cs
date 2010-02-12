/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
 *
 * Windar: Playdar for Windows
 *
 * Windar is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Windar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 ************************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using log4net;

namespace Windar.Common
{
    public class SimplePropertiesFile
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public Dictionary<string, Dictionary<string, string>> Sections { get; set; }

        readonly Dictionary<string, string> _section;
        readonly string _filename;

        public SimplePropertiesFile(string filename)
        {
            _filename = filename;
            if (Log.IsDebugEnabled) Log.Debug("Filename = " + _filename);
            Sections = new Dictionary<string, Dictionary<string, string>>();

            if (!File.Exists(_filename)) return;

            foreach (var row in File.ReadAllLines(_filename))
            {
                if (row.Length <= 0) continue;
                var skip = false;
                switch (row[0])
                {
                    case '[':
                        var section = row.Substring(1, row.IndexOf(']') - 1);
                        if (Log.IsDebugEnabled) Log.Debug("Section name = " + section);
                        _section = new Dictionary<string, string>();
                        Sections.Add(section, _section);
                        skip = true;
                        break;
                    case '#':
                    case ';':
                        if (Log.IsDebugEnabled) Log.Debug("Ignoring: " + row);
                        skip = true;
                        break;
                }
                if (skip) continue;
                if (_section == null)
                {
                    _section = new Dictionary<string, string>();
                    Sections.Add("undefined", _section);
                }
                var spit = row.Split('=');
                var name = spit[0].Trim();
                var value = spit[1].Trim();
                if (Log.IsDebugEnabled) Log.Debug(name + " = " + value);
                _section.Add(name, value);
            }
        }

        public void Save()
        {
            var str = new StringBuilder();
            foreach (var section in Sections)
            {
                str.Append('[').Append(section.Key).Append("]\n");
                foreach (var p in section.Value)
                    str.Append(p.Key).Append(" = ").Append(p.Value).Append('\n');
            }

            // Write the configuration file.
            if (File.Exists(_filename)) File.Delete(_filename);
            var file = new FileStream(_filename, FileMode.Create, FileAccess.Write);
            var sw = new StreamWriter(file);
            sw.Write(str.ToString());
            sw.Close();
            file.Close();
        }
    }
}
