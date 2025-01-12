namespace PollApp.Mobile.Helpers
{
    public static class Settings
    {
        // App Settings
        public static class App
        {
            public const string Name = "PollApp";
            public const string Version = "1.0.0";
        }

        // User Settings
        public static class User
        {
            public static string Username 
            { 
                get => Preferences.Get(nameof(Username), string.Empty);
                set => Preferences.Set(nameof(Username), value);
            }

            public static string Email 
            { 
                get => Preferences.Get(nameof(Email), string.Empty);
                set => Preferences.Set(nameof(Email), value);
            }

            public static bool IsLoggedIn 
            { 
                get => Preferences.Get(nameof(IsLoggedIn), false);
                set => Preferences.Set(nameof(IsLoggedIn), value);
            }
        }

        // Theme Settings
        public static class Theme
        {
            public static string CurrentTheme 
            { 
                get => Preferences.Get(nameof(CurrentTheme), "Light");
                set => Preferences.Set(nameof(CurrentTheme), value);
            }

            public static bool IsDarkMode 
            { 
                get => CurrentTheme == "Dark";
                set => CurrentTheme = value ? "Dark" : "Light";
            }
        }

        // API Settings
        public static class Api
        {
            public static string BaseUrl 
            { 
                get => Preferences.Get(nameof(BaseUrl), "https://api.pollapp.com/");
                set => Preferences.Set(nameof(BaseUrl), value);
            }

            public static string AuthToken 
            { 
                get => SecureStorage.GetAsync("auth_token").Result;
                set => SecureStorage.SetAsync("auth_token", value);
            }
        }

        // Notification Settings
        public static class Notifications
        {
            public static bool AreEnabled 
            { 
                get => Preferences.Get(nameof(AreEnabled), true);
                set => Preferences.Set(nameof(AreEnabled), value);
            }
        }

        // Clear all settings
        public static void ClearAll()
        {
            Preferences.Clear();
            SecureStorage.RemoveAll();
        }
    }
}