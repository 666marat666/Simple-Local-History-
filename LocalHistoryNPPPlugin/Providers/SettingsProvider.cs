using Newtonsoft.Json;
using LocalHistoryNPPPlugin.Providers.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace LocalHistoryNPPPlugin.Providers
{
    public static class SettingsProvider //: ISettingsProvider
    {
        static Dictionary<string, string> settings = new Dictionary<string, string>();
        static string pathToFileWithSettings;

        static SettingsProvider()
        {
            pathToFileWithSettings = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "LocalHistoryNPPPlugin.json");
            if (File.Exists(pathToFileWithSettings))
                LoadSettings(pathToFileWithSettings);
            else SaveSettings();
        }

        public static void AddSetting(string key, string value)
        {
            if (settings.ContainsKey(key))
                settings[key] = value;
            else
                settings.Add(key, value);
        }

        public static int GetIntSettingByName(string name, int defaultValue)
        {
            if (settings.ContainsKey(name))
                return Int32.Parse(settings[name]);
            else return defaultValue;
        }

        public static string GetStrSettingByName(string name, string defaultValue)
        {
            if (settings.ContainsKey(name))
                return settings[name];
            else return defaultValue;
        }

        public static void SaveSettings()
        {
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(pathToFileWithSettings, json);
        }

        public static void LoadSettings(string pathToFile)
        {
            string json = File.ReadAllText(pathToFile);
            settings = JsonConvert.DeserializeObject<Dictionary<string,string>>(json);
        }
    }
}
