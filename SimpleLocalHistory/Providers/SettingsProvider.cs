using Newtonsoft.Json;
using SimpleLocalHistory.Core.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleLocalHistory.Providers
{
    public class SettingsProvider : ISettingsProvider
    {
        Dictionary<string, string> settings = new Dictionary<string, string>();
        string pathToFileWithSettings;

        public SettingsProvider(string pathToFileWithSettings)
        {
            this.pathToFileWithSettings = pathToFileWithSettings;
            if (File.Exists(pathToFileWithSettings))
                LoadSettings(pathToFileWithSettings);
            else
                SaveSettings(pathToFileWithSettings);
        }

        public void AddSetting(string key, string value)
        {
            if (settings.ContainsKey(key))
                settings[key] = value;
            else
                settings.Add(key, value);
        }

        public int GetIntSettingByName(string name, int defaultValue)
        {
            if (settings.ContainsKey(name))
                return Int32.Parse(settings[name]);
            else return defaultValue;
        }

        public string GetStrSettingByName(string name, string defaultValue)
        {
            if (settings.ContainsKey(name))
                return settings[name];
            else return defaultValue;
        }

        public void SaveSettings(string pathToFile)
        {
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(pathToFile, json);
        }

        public void LoadSettings(string pathToFile)
        {
            string json = File.ReadAllText(pathToFile);
            settings = JsonConvert.DeserializeObject<Dictionary<string,string>>(json);
        }

        ~SettingsProvider()
        {
            SaveSettings(this.pathToFileWithSettings);
        }
    }
}
