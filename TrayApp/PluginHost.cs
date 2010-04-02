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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using log4net;
using Windar.Common;
using Windar.PluginAPI;

namespace Windar.TrayApp
{
    class PluginHost : IPluginHost
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public List<IPlugin> Plugins { get; set; }
        public WindarPaths Paths { get; set; }

        public Credentials ScrobblerCredentials
        {
            get
            {
                return Program.Instance.Config.Main.ScrobblerCredentials;
            }
            set
            {
                Program.Instance.Config.Main.ScrobblerCredentials = value;
                Program.Instance.Config.Main.Save();
            }
        }

        public PluginHost(WindarPaths paths)
        {
            Paths = paths;
        }

        public void Load()
        {
            if (Log.IsDebugEnabled) Log.Debug("Loading plugins.");
            Plugins = GetPlugins<IPlugin>();
            foreach (var plugin in Plugins)
            {
                plugin.Host = this;
                plugin.Load();

                if (Log.IsDebugEnabled) Log.Debug("Loaded plugin: " + plugin.GetType().Name);
            }
        }

        public List<T> GetPlugins<T>()
        {
            var path = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
            return GetPlugins<T>(path);
        }

        public List<T> GetPlugins<T>(string path)
        {
            if (Log.IsInfoEnabled) Log.Info("Loading plugins.");
            if (Log.IsDebugEnabled) Log.Debug("Plugins path = " + path);
            var files = Directory.GetFiles(path, "*Plugin.dll");
            if (Log.IsDebugEnabled) Log.Debug("Plugin count = " + files.Length);
            var list = new List<T>();
            foreach (var file in files)
            {
                var assembly = Assembly.LoadFile(file);
                if (Log.IsDebugEnabled) Log.Debug("Loaded assembly = " + file);
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsClass || type.IsNotPublic) continue;
                    if (!((IList)type.GetInterfaces()).Contains(typeof(T))) continue;
                    try
                    {
                        list.Add((T) Activator.CreateInstance(type));
                        if (Log.IsInfoEnabled) Log.Info("Loaded " + type.Name);
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        if (Log.IsErrorEnabled)
                        {
                            var sb = new StringBuilder();
                            sb.Append("Loader Exception");
                            foreach (var e in ex.LoaderExceptions)
                                sb.Append('\n').Append(e.Message);
                            Log.Error(sb.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Log.IsErrorEnabled)
                            Log.Error("Exception when reading plugins.", ex);
                    }
                }
            }
            return list;
        }

        public void Shutdown()
        {
            if (Plugins == null) return;
            foreach (var plugin in Plugins)
            {
                plugin.Shutdown();
            }
        }

        public void AddTabPage(UserControl control, string title)
        {
            var tab = new TabPage {Text = title};
            tab.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            Program.Instance.MainForm.mainTabControl.Controls.Add(tab);

            // Keep the log tab at the end.
            var tabs = Program.Instance.MainForm.mainTabControl.TabPages;
            foreach (var page in tabs)
            {
                var tabPage = (TabPage) page;
                if (tabPage.Name != "logTabPage") continue;
                tabs.Remove(tabPage);
                tabs.Add(tabPage);
            }
        }

        public void AddConfigurationPage(ConfigTabContent control, string title)
        {
            var tab = new TabPage { Text = title };
            tab.Controls.Add(control);
            tab.Padding = new Padding(3);
            tab.BackColor = Color.White;
            control.Dock = DockStyle.Fill;
            Program.Instance.MainForm.optionsTabControl.Controls.Add(tab);
        }

        public void ApplyChangesRequiresDaemonRestart()
        {
            Program.ShowApplyChangesDialog();
        }
    }
}
