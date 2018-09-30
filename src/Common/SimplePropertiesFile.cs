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

        Dictionary<string, Dictionary<string, string>> _sections;

        public Dictionary<string, Dictionary<string, string>> Sections
        {
            get { return _sections; }
            set { _sections = value; }
        }

        readonly Dictionary<string, string> _section;
        readonly string _filename;

        public SimplePropertiesFile(string filename)
        {
            _filename = filename;
            if (Log.IsDebugEnabled) Log.Debug("Filename = " + _filename);
            Sections = new Dictionary<string, Dictionary<string, string>>();

            if (!File.Exists(_filename)) return;

            foreach (string row in File.ReadAllLines(_filename))
            {
                if (row.Length <= 0) continue;
                bool skip = false;
                switch (row[0])
                {
                    case '[':
                        string section = row.Substring(1, row.IndexOf(']') - 1);
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
                string[] spit = row.Split('=');
                string name = spit[0].Trim();
                string value = spit[1].Trim();
                if (Log.IsDebugEnabled) Log.Debug(name + " = " + value);
                _section.Add(name, value);
            }
        }

        public void Save()
        {
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, Dictionary<string, string>> section in Sections)
            {
                str.Append('[').Append(section.Key).Append("]\n");
                foreach (KeyValuePair<string, string> p in section.Value)
                    str.Append(p.Key).Append(" = ").Append(p.Value).Append('\n');
            }

            // Write the configuration file.
            if (File.Exists(_filename)) File.Delete(_filename);
            FileStream file = new FileStream(_filename, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);
            sw.Write(str.ToString());
            sw.Close();
            file.Close();
        }
    }
}
