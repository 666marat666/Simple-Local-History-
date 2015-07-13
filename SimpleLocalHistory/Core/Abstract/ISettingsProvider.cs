using System;
namespace SimpleLocalHistory.Core.Abstract
{
    public interface ISettingsProvider
    {
        int GetIntSettingByName(string name, int defaultValue);
        string GetStrSettingByName(string name, string defaultValue);
        void AddSetting(string key, string value);
    }
}
