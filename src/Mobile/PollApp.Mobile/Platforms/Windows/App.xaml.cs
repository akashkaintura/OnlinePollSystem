using Microsoft.UI.Xaml;

namespace PollApp.Mobile.Platforms.Windows;

public partial class App : MauiWinUIApplication
{
    public App()
    {
        this.InitializeComponent();
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        // Windows-specific initialization
        InitializeWindowsPlatform();
    }

    private void InitializeWindowsPlatform()
    {
        // Check for first-time launch
        if (!IsAppFirstTimeLaunch())
        {
            SetupInitialWindowsSettings();
        }

        // Configure app-specific settings
        ConfigureAppSettings();
    }

    private bool IsAppFirstTimeLaunch()
    {
        return Windows.Storage.ApplicationData.Current.LocalSettings
            .Values["first_launch"] == null;
    }

    private void SetupInitialWindowsSettings()
    {
        // Mark first launch as completed
        Windows.Storage.ApplicationData.Current.LocalSettings
            .Values["first_launch"] = false;
        
        // Initial configuration
        Settings.Theme.CurrentTheme = "Light";
        Settings.Notifications.AreEnabled = true;
    }

    private void ConfigureAppSettings()
    {
        // Windows-specific app configuration
        ConfigureWindowSize();
        ConfigureTheme();
    }

    private void ConfigureWindowSize()
    {
        // Set default window size
        var preferredSize = new Windows.Foundation.Size(1024, 768);
        
        if (MainWindow != null)
        {
            MainWindow.Width = preferredSize.Width;
            MainWindow.Height = preferredSize.Height;
        }
    }

    private void ConfigureTheme()
    {
        // Set Windows app theme
        var theme = Settings.Theme.CurrentTheme;
        
        if (theme == "Dark")
        {
            // Apply dark theme
            MainWindow.SystemBackdrop = new MicaBackdrop();
        }
        else
        {
            // Apply light theme
            MainWindow.SystemBackdrop = new DefaultBackdrop();
        }
    }
}