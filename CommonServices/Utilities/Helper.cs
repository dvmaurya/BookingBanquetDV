using Windows.Storage;

namespace CommonServices.Utilities
{
    public static class Helper
    {
        public static T GetLocalSettingsValue<T>(string name)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(name) && ApplicationData.Current.LocalSettings.Values[name] != null)
                return (T)ApplicationData.Current.LocalSettings.Values[name];

            return default(T);
        }

        public static void SetLocalSetting<T>(string name, T value)
        {
            ApplicationData.Current.LocalSettings.Values[name] = value;
        }
    }
}
