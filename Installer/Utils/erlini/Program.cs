/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
 *
 * Windar: Playdar for Windows
 *
 * Windar is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
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

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Windar.Erlini
{
    static class Program
    {
        private const string Newline = "\r\n";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var cmdPath = Application.StartupPath;
                //Info("windarPath = " + cmdPath);
                var ini = BuildIniFileContent(EscapePath(ParentPath(cmdPath)));
                var filename = cmdPath + @"\erl.ini";
                //Info("filename = " + filename);
                WriteFile(filename, ini);
            }
            catch(Exception ex)
            {
                Info("Error: " + ex.Message);
            }
        }

        private static void Info(string msg)
        {
            MessageBox.Show(msg, "Erlini", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string BuildIniFileContent(string cmdPath)
        {
            var result = new StringBuilder();
            result.Append("[erlang]").Append(Newline);
            result.Append("Bindir=").Append(cmdPath).Append(EscapePath(@"\bin")).Append(Newline);
            result.Append("Progname=erl").Append(Newline);
            result.Append("Rootdir=").Append(cmdPath).Append(Newline);
            return result.ToString();
        }

        private static string EscapePath(string path)
        {
            return path.Replace(@"\", @"\\");
        }

        private static string ParentPath(string path)
        {
            return path.Substring(0, path.LastIndexOf('\\'));
        }

        private static void WriteFile(string filename, string text)
        {
            var fout = new FileStream(filename, FileMode.Create);
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(text);
            for (var i = 0; i < bytes.Length; i++)
            {
                fout.WriteByte(bytes[i]);
            }
            fout.Close();
        }
    }
}
