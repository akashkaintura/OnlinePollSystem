namespace PollApp.Mobile;

public partial class App : Application
{
    public App(
        IPollService pollService, 
        IAuthService authService)
    {
        InitializeComponent();

        // Set up shell navigation
        MainPage = new AppShell();
    }

    protected override async void OnStart()
    {
        // Check if user is already authenticated
        var authService = Handler.MauiContext.Services.GetService<IAuthService>();
        
        try 
        {
            var isAuthenticated = await authService.IsAuthenticatedAsync();
            
            if (isAuthenticated)
            {
                // Navigate to main app page
                await Shell.Current.GoToAsync("//main");
            }
            else 
            {
                // Navigate to login page
                await Shell.Current.GoToAsync("//login");
            }
        }
        catch 
        {
            // Navigate to login page if any error occurs
            await Shell.Current.GoToAsync("//login");
        }
    }
}