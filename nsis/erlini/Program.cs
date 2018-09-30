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
