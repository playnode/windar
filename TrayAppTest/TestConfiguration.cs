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
using log4net;
using NUnit.Framework;
using Windar.TrayApp.Configuration;
using Windar.TrayApp.Configuration.Parser;

namespace Windar.TrayApp
{
    [TestFixture]
    public class TestConfiguration
    {
        #region Test support code

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

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

        #endregion

        [Test]
        public void ErlangTermsDocumentClass()
        {
            try
            {
                var doc = new ErlangTermsDocument();
                doc.Load(new FileInfo(TestConfigurationPath + "playdar.conf.example"));
            }
            catch (Exception ex)
            {
                Log.Error("Exception", ex);
                throw new Exception(ex.Message, ex);
            }
        }

        [Test]
        public void MainConfigFileClass()
        {
            try
            {
                var config = new MainConfigFile();
                config.Load(TestConfigurationPath + "playdar.conf.example");
                Log.Info("Current name = " + config.NodeName);
                config.NodeName = "TEST";
                Log.Info("\n" + config);
            }
            catch (Exception ex)
            {
                Log.Error("Exception", ex);
                throw new Exception(ex.Message, ex);
            }
        }

        [Test]
        public void TcpConfigFileClass()
        {
            try
            {
                var config = new MainConfigFile();
                config.Load(TestConfigurationPath + "playdartcp.conf.example");
            }
            catch (Exception ex)
            {
                Log.Error("Exception", ex);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
