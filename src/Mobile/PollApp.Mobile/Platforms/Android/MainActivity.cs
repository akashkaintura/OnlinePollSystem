using Android.App;
using Android.Content.PM;
using Android.OS;

namespace PollApp.Mobile.Platforms.Android;

[Activity(Label = "PollApp", 
    Theme = "@style/Maui.SplashTheme", 
    MainLauncher = true, 
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Platform-specific initialization
        InitializePlatform();
    }

    private void InitializePlatform()
    {
        // Custom Android-specific initializations
        // Example: Checking for first-time app launch
        if (!IsAppFirstTimeLaunch())
        {
            SetupInitialAppSettings();
        }

        // Request necessary permissions
        RequestRequiredPermissions();
    }

    private bool IsAppFirstTimeLaunch()
    {
        return Preferences.Get("first_launch", true);
    }

    private void SetupInitialAppSettings()
    {
        // First-time app setup
        Preferences.Set("first_launch", false);
        
        // Initial configuration
        Settings.Theme.CurrentTheme = "Light";
        Settings.Notifications.AreEnabled = true;
    }

    private void RequestRequiredPermissions()
    {
        // Request runtime permissions
        if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Internet) 
            != Permission.Granted)
        {
            ActivityCompat.RequestPermissions(this, 
                new[] { Manifest.Permission.Internet }, 
                REQUEST_INTERNET_PERMISSION);
        }
    }

    private const int REQUEST_INTERNET_PERMISSION = 1;

    public override void OnRequestPermissionsResult(
        int requestCode, 
        string[] permissions, 
        Permission[] grantResults)
    {
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        if (requestCode == REQUEST_INTERNET_PERMISSION)
        {
            if (grantResults.Length > 0 && 
                grantResults[0] == Permission.Granted)
            {
                // Internet permission granted
            }
            else
            {
                // Permission denied
                Toast.MakeText(this, "Internet permission is required", ToastLength.Long).Show();
            }
        }
    }
}