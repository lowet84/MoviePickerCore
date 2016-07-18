using System;
using System.IO;
using MoviePicker.Utility;
using MoviePickerCore.Model;
using Newtonsoft.Json;

namespace MoviePickerCore.Utility
{
    public static class SettingsHelper
    {
        private const string SettingsFile = "settings.json";
        public static bool SetPassword(string password)
        {
            var settings = GetSettings();
            settings.DelugePassword = password;
            SaveSettings(settings);
            return true;
        }

        private static Settings GetSettings()
        {
            if (!File.Exists(SettingsFile))
            {
                return new Settings();
            }
            var settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsFile));
            return settings;
        }

        public static string GetDelugePass()
        {
            return GetSettings().DelugePassword;
        }

        private static void SaveSettings(Settings settings)
        {
            var json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(SettingsFile,json);
        }

        private static string ProtectUsingKey(string plainText, string key)
        {
            return string.Empty;
            //return StringCipher.Encrypt(plainText, key);
        }

        private static string UnProtectUsingKey(string plainText, string key)
        {
            return string.Empty;
            //return StringCipher.Decrypt(plainText, key);
        }
    }
}
