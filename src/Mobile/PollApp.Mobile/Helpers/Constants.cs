namespace PollApp.Mobile.Helpers
{
    public static class Constants
    {
        // API Base URL
        public static string ApiBaseUrl { get; } = 
            DeviceInfo.Platform == DevicePlatform.Android 
                ? "http://10.0.2.2:5000/api/" 
                : "http://localhost:5000/api/";

        // Application-wide constants
        public static class App
        {
            public const string AppName = "PollApp";
            public const int MaxPollOptions = 10;
            public const int MinPollOptions = 2;
        }

        // Secure Storage Keys
        public static class SecureStorageKeys
        {
            public const string AuthToken = "auth_token";
            public const string RefreshToken = "refresh_token";
        }
    }
}