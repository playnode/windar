using System;
using System.IO;
using System.Reflection;
using log4net;
using NUnit.Framework;
using Playnode.ErlangTerms.Parser;
using Windar.TrayApp.Configuration;

namespace Windar.TrayApp
{
    [TestFixture]
    public class TestConfiguration
    {
        #region Test support code

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        static string ProjectRootPath
        {
            get
            {
                string a = (new Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
                string b = a.Substring(0, a.LastIndexOf('/')); // Debug folder.
                string c = b.Substring(0, b.LastIndexOf('/')); // bin folder.
                string d = c.Substring(0, c.LastIndexOf('/')); // TrayAppTest folder.
                return d + '/';
            }
        }

        static string TestConfigurationPath
        {
            get
            {
                return ProjectRootPath + "TestConfiguration/";
            }
        }

        [SetUp]
        public void SetUp()
        {
            string filename = ProjectRootPath + "Log4net.xml";
            FileInfo configFile = new FileInfo(filename);
            log4net.Config.XmlConfigurator.Configure(configFile);
        }

        #endregion

        [Test]
        public void ErlangTermsDocumentClass()
        {
            try
            {
                ErlangTermsDocument doc = new ErlangTermsDocument();
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
                MainConfigFile config = new MainConfigFile();
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
                TcpConfigFile config = new TcpConfigFile();
                config.Load(new FileInfo(TestConfigurationPath + "playdartcp.conf.example"));
                Log.Info("Configuration file loaded.");

                PeerInfo peer = config.GetPeerInfo("192.168.1.10", 60211);
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

                //config.SetPeerInfo("192.168.1.10", 60211, true);

                //config.RemovePeer("10.1.1.45", 60211);
                //config.RemovePeer("192.168.1.10", 60211);

                config.RemovePeer("playnode.k-os.net", 60211);
                config.RemovePeer("k-os.podzone.net", 60211);

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
