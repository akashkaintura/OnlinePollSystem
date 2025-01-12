using Foundation;
using UIKit;

namespace PollApp.Mobile.Platforms.iOS;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // Perform additional iOS-specific initialization
        InitializeiOSPlatform();

        return base.FinishedLaunching(application, launchOptions);
    }

    private void InitializeiOSPlatform()
    {
        // Check for first-time launch
        if (!IsAppFirstTimeLaunch())
        {
            SetupInitialiOSSettings();
        }

        // Configure push notifications
        ConfigurePushNotifications();
    }

    private bool IsAppFirstTimeLaunch()
    {
        return NSUserDefaults.StandardUserDefaults.BoolForKey("first_launch");
    }

    private void SetupInitialiOSSettings()
    {
        NSUserDefaults.StandardUserDefaults.SetBool(true, "first_launch");
        
        // Initial configuration
        Settings.Theme.CurrentTheme = "Light";
        Settings.Notifications.AreEnabled = true;
    }

    private void ConfigurePushNotifications()
    {
        // iOS Push Notification Configuration
        if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
        {
            // iOS 10 notification handling
            UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();
        }

        // Request notification permissions
        RequestNotificationPermissions();
    }

    private void RequestNotificationPermissions()
    {
        UNUserNotificationCenter.Current.RequestAuthorization(
            UNAuthorizationOptions.Alert | 
            UNAuthorizationOptions.Badge | 
            UNAuthorizationOptions.Sound, 
            (approved, error) =>
            {
                // Handle notification permission result
                if (approved)
                {
                    InvokeOnMainThread(() =>
                    {
                        UIApplication.SharedApplication.RegisterForRemoteNotifications();
                    });
                }
            });
    }

    // Notification Delegate
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(
            UNUserNotificationCenter center, 
            UNNotification notification, 
            Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Handle foreground notification presentation
            completionHandler(UNNotificationPresentationOptions.Banner);
        }
    }
}