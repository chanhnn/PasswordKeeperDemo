// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace PasswordKeeperDemo.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        /**/
        private const string IsRequirePasswordSettingKey = "IsRequirePassword";

        private static readonly bool IsRequirePasswordSettingDefault = false;

        public static bool IsRequirePasswordSetting
        {
            get => AppSettings.GetValueOrDefault(IsRequirePasswordSettingKey, IsRequirePasswordSettingDefault);
            set => AppSettings.AddOrUpdateValue(IsRequirePasswordSettingKey, value);
        }

        /**/
        private const string PasswordSettingKey = "Password";

        private static readonly string PasswordSettingDefault = string.Empty;

        public static string PasswordSetting
        {
            get => AppSettings.GetValueOrDefault(PasswordSettingKey, PasswordSettingDefault);
            set => AppSettings.AddOrUpdateValue(PasswordSettingKey, value);
        }
    }
}