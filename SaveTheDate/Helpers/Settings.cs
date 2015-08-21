// Helpers/Settings.cs
using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;

namespace SaveTheDate.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        const string RegisteredKey = "registered_key";
        const bool RegisteredDefault = false;
        const string RegisteredEmailKey = "registered_email_key";
        static readonly string RegisteredEmailDefault = string.Empty;
        const string CalendarKey = "calendar_key";
        const bool CalendarDefault = false;

        #endregion
        public static long GetEventId(string id)
        {
            return AppSettings.GetValueOrDefault<long> (id, (long)-1);
        }

        public static void AddEventId(string id, long eventId)
        {
            AppSettings.AddOrUpdateValue<long>(id, eventId);
        }

        public static bool IsRegistered
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>(RegisteredKey, RegisteredDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(RegisteredKey, value);
            }
        }

        public static string RegisteredEmail
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(RegisteredEmailKey, RegisteredEmailDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(RegisteredEmailKey, value);
            }
        }

        public static bool AddedToCalendar
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>(CalendarKey, CalendarDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(CalendarKey, value);
            }
        }

    }
}