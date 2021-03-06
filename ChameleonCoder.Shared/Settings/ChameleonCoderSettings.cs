﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace ChameleonCoder.Settings
{
    [SettingsProvider(typeof(CCSettingsProvider)), System.Runtime.InteropServices.ComVisible(false)]
    public sealed class ChameleonCoderSettings : ApplicationSettingsBase
    {
        private ChameleonCoderSettings()
        {
            var args = new List<string>(Environment.GetCommandLineArgs());
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ChameleonCoder.config.xml");

            if (args.Contains("--config") && args.IndexOf("--config") + 1 < args.Count)
            {
                file = args[args.IndexOf("--config") + 1];
                
            }

            if (!File.Exists(file))
            {
                File.Create(file).Close();
                File.AppendAllText(file, "<settings/>");
            }
            (Providers[CCSettingsProvider.providerName] as CCSettingsProvider).SettingsFile = file;

            PropertyChanged += (s, e) => Save();
        }

        public static ChameleonCoderSettings Default
        {
            get
            {
                lock (defaultInstance)
                {
                    return defaultInstance;
                }
            }
        }

        private static readonly ChameleonCoderSettings defaultInstance = new ChameleonCoderSettings();

        [DefaultSettingValue("1033")]
        [UserScopedSetting]
        public int Language
        {
            get { return (int)this["Language"]; }
            set { this["Language"] = value; }
        }

        [DefaultSettingValue("Arial")]
        [UserScopedSetting]
        public string CodeFont
        {
            get { return (string)this["CodeFont"]; }
            set { this["CodeFont"] = value; }
        }

        [DefaultSettingValue("10")]
        [UserScopedSetting]
        public int CodeFontSize
        {
            get { return (int)this["CodeFontSize"]; }
            set { this["CodeFontSize"] = value; }
        }

        [DefaultSettingValue("<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><string>e6662af6d0fd45bda2ab8784eda3079d</string></ArrayOfString>")]
        [UserScopedSetting]
        public System.Collections.Specialized.StringCollection InstalledPlugins
        {
            get { return (System.Collections.Specialized.StringCollection)this["InstalledPlugins"]; }
            set { this["InstalledPlugins"] = value; }
        }
    }
}
