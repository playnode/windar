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
                Log.Info("Erlang terms document loaded.");
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
                config.Load(new FileInfo(TestConfigurationPath + "playdar.conf.example"));
                Log.Info("Configuration file loaded.");

                // Change name.
                Log.Info("Current name = " + config.Name);
                config.Name = "TEST";

                // Change crossdomain.
                Log.Info("Current crossdomain = " + config.CrossDomain);
                config.CrossDomain = false;

                // Change authdbdir.
                Log.Info("Current authdbdir = " + config.AuthDbDir);
                config.AuthDbDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Windar\";

                // Change explain.
                Log.Info("Current explain = " + config.Explain);
                config.Explain = true;

                // Change port.
                Log.Info("Current port = " + config.WebPort);
                config.WebPort = 3100;

                // Change max.
                Log.Info("Current max = " + config.WebMax);
                config.WebMax = 99;

                // Change ip.
                Log.Info("Current ip = " + config.WebIp);
                config.WebIp = "198.168.1.12";

                // Change docroot.
                Log.Info("Current docroot = " + config.WebDocRoot);
                config.WebDocRoot = "priv/www2";

                // Change {library, dbdir}
                Log.Info("Current {library, dbdir} = " + config.LibraryDbDir);
                config.LibraryDbDir = "/tmp";

                // Add and remove script.
                config.AddScript("Test script 1");
                config.AddScript("Test script 2");
                config.RemoveScript("Test script 1");
                //config.RemoveScript("Test script 2");
                //config.AddScript("Test script 1");
                //config.AddScript("Test script 2");
                //config.RemoveScript("Test script");

                // Add and remove module from blacklist.
                config.BlockModule("Test module 1");
                config.BlockModule("Test module 2");
                config.UnblockModule("Test module 1");
                config.UnblockModule("aolmusic");

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
                var config = new TcpConfigFile();
                config.Load(new FileInfo(TestConfigurationPath + "playdartcp.conf.example"));
                Log.Info("Configuration file loaded.");

                var peer = config.GetPeerInfo("192.168.1.10", 60211);
                Log.Info("PeerInfo = " + peer);

                //config.SetPeerInfo("192.168.1.10", 60211, true);
                //config.SetPeerInfo("10.1.1.10", 60211, false);
                //config.SetPeerInfo("10.1.1.11", 60211, false);
                //config.SetPeerInfo("10.1.1.12", 60211, false);
                //config.SetPeerInfo("10.1.1.13", 60211, false);
                //config.RemovePeer("10.1.1.11", 60211);
                //config.RemovePeer("10.1.1.10", 60211);
                //config.RemovePeer("10.1.1.12", 60211);
                //config.RemovePeer("10.1.1.13", 60211);
                //config.RemovePeer("192.168.1.10", 60211);

                config.SetPeerInfo("192.168.1.10", 60211, true);

                config.RemovePeer("10.1.1.45", 60211);
                config.RemovePeer("k-os.podzone.net", 60211);
                config.RemovePeer("playnode.k-os.net", 60211);
                config.RemovePeer("192.168.1.10", 60211);
                
                Log.Info("\n" + config);
            }
            catch (Exception ex)
            {
                Log.Error("Exception", ex);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
