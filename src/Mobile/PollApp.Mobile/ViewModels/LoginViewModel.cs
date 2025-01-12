using System.Windows.Input;
using PollApp.Mobile.Services.Interfaces;
using PollApp.Mobile.Models;

namespace PollApp.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        
        private string _email;
        private string _password;
        private bool _isLoginEnabled;

        public string Email
        {
            get => _email;
            set 
            {
                SetProperty(ref _email, value);
                ValidateLogin();
            }
        }

        public string Password
        {
            get => _password;
            set 
            {
                SetProperty(ref _password, value);
                ValidateLogin();
            }
        }

        public bool IsLoginEnabled
        {
            get => _isLoginEnabled;
            set => SetProperty(ref _isLoginEnabled, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            Title = "Login";

            LoginCommand = new Command(async () => await Login(), () => IsLoginEnabled);
            RegisterCommand = new Command(async () => await NavigateToRegister());
        }

        private void ValidateLogin()
        {
            IsLoginEnabled = !string.IsNullOrWhiteSpace(Email) && 
                             !string.IsNullOrWhiteSpace(Password) && 
                             Email.Contains("@");
        }

        private async Task Login()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var user = await _authService.LoginAsync(Email, Password);
                
                // Store user token or session
                await SecureStorage.SetAsync("auth_token", user.Token);

                // Navigate to main app page
                await Shell.Current.GoToAsync("//main");
            }
            catch (Exception ex)
            {
                await HandleError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task NavigateToRegister()
        {
            await Shell.Current.GoToAsync("//register");
        }
    }
}