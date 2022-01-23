using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyCacheCleaner.Cli
{
    internal class SettingsHandler
    {
        private static readonly string[] KEYS = new string[]{"InstallPath", "Version"};
        internal enum Ekeys
        {
            InstallPath,
            Version
        }

        internal static string? Read(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings[key];
        }

        internal static string? Read(Ekeys id)
        {
            return Read(KEYS[((int)id)]);
        }

        internal static bool CanStart()
        {
            foreach (var key in KEYS)
            {
                if (Read(key) == null) return false;
            }
            return true;
        }

        internal static void AddUpdate(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
    }
}
