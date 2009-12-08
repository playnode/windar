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
using System.IO;
using System.Reflection;
using System.Text;
using log4net;
using NUnit.Framework;
using Windar.TrayApp.Configuration.Parser;
using Windar.TrayApp.Configuration.Parser.Tokens;
using List = Windar.TrayApp.Configuration.Parser.Tokens.List;
using String = Windar.TrayApp.Configuration.Parser.Tokens.String;

namespace Windar.TrayApp
{
    [TestFixture]
    public class TestConfigurationParser
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        #region Test support code

        private static string ProjectRootPath
        {
            get
            {
                var a = (new Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
                var b = a.Substring(0, a.LastIndexOf('/')); // Debug folder.
                var c = b.Substring(0, b.LastIndexOf('/')); // bin folder.
                var d = c.Substring(0, c.LastIndexOf('/')); // TrayAppTest folder.
                return d + '/';
            }
        }

        private static string TestConfigurationPath
        {
            get
            {
                return ProjectRootPath + "TestConfiguration/";
            }
        }

        [SetUp]
        public void SetUp()
        {
            var filename = ProjectRootPath + "Log4net.xml";
            var configFile = new FileInfo(filename);
            log4net.Config.XmlConfigurator.Configure(configFile);
        }

        private static string GetTextFromFile(string filename)
        {
            var result = new StringBuilder();
            string line;
            TextReader reader = new StreamReader(filename);
            while ((line = reader.ReadLine()) != null) result.Append(line).Append("\n");
            reader.Close();
            return result.ToString();
        }

        private static void TestConfigurationText(string text)
        {
            try
            {
                var stream = new ParserInputStream(text);
                var parser = new ErlangTermsParser(stream);
                var config = new System.Collections.Generic.List<ParserToken>();

                ParserToken token;
                while ((token = parser.NextToken()) != null)
                {
                    config.Add(token);

                    if (token is Comment)
                    {
                        if (Log.IsInfoEnabled)
                        {
                            var str = token.ToString();
                            Log.Info("Token, Comment: " + str.Substring(0, str.Length - 1));
                        }
                    }
                    else if (token is EndTerm)
                    {
                        if (Log.IsInfoEnabled)
                        {
                            Log.Info("Token, EndTerm.");
                        }
                    }
                    else if (token is NumericExpression)
                    {
                        if (Log.IsInfoEnabled)
                        {
                            var str = token.ToString();
                            if (str.IndexOf('\n') > 0) Log.Info("Token, NumericExpression (multi-line)\n" + str);
                            else Log.Info("Token, NumericExpression: " + str);
                        }
                    }
                    else if (token is List)
                    {
                        if (Log.IsInfoEnabled)
                        {
                            var str = token.ToString();
                            if (str.IndexOf('\n') > 0) Log.Info("Token, List (multi-line)\n" + str);
                            else Log.Info("Token, List: " + str);
                        }
                    }
                    else if (token is String)
                    {
                        if (Log.IsInfoEnabled)
                        {
                            var str = token.ToString();
                            if (str.IndexOf('\n') > 0) Log.Info("Token, String (multi-line)\n" + str);
                            else Log.Info("Token, String: \"" + str + '"');
                        }
                    }
                    else if (token is Tuple)
                    {
                        if (Log.IsInfoEnabled)
                        {
                            var str = token.ToString();
                            if (str.IndexOf('\n') > 0) Log.Info("Token, Tuple (multi-line)\n" + str);
                            else Log.Info("Token, Tuple: " + str);
                        }
                    }
                    else if (token is Whitespace)
                    {
                        if (Log.IsInfoEnabled)
                        {
                            Log.Info("Token, Whitespace: " + ((Whitespace) token).ToEscapedString());
                        }
                    }
                    else
                    {
                        if (Log.IsInfoEnabled)
                        {
                            Log.Info("Token, Unrecognised!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled)
                {
                    Log.Error("Exception", ex);
                }
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        [Test]
        public void ParseCurrentPlaydarConfExample()
        {
            TestConfigurationText(GetTextFromFile(TestConfigurationPath + "playdar.conf.example"));
        }

        [Test]
        public void ParseSimplerPlaydarConfExample()
        {
            TestConfigurationText(GetTextFromFile(TestConfigurationPath + "playdar.conf"));
        }
    }
}
