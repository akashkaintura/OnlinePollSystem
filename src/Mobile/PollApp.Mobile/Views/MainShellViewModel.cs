public class MainShellViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    public ICommand LogoutCommand { get; }

    public MainShellViewModel(IAuthService authService)
    {
        _authService = authService;
        
        LogoutCommand = new Command(async () => await LogoutAsync());
    }

    private async Task LogoutAsync()
    {
        try 
        {
            await _authService.LogoutAsync();
            
            // Navigate back to login page
            await Shell.Current.GoToAsync("//login");
        }
        catch (Exception ex)
        {
            // Handle logout error
            await HandleError(ex);
        }
    }
}