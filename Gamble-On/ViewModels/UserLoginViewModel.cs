using Gamble_On.Services;
using System.Windows.Input;
using Gamble_On.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Gamble_On.ViewModels
{
    public class UserLoginViewModel : ViewModelBase
    {
        private string _password;
        private string _email;
        private readonly IUserService _userService;

        public ICommand LoginCommand { get; }

        public UserLoginViewModel(IUserService userService)
        {
            _userService = userService;
            LoginCommand = new Command(async () => await OnLoginClicked());
        }
        // Get&Set properties
        public string email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        // async functions

        // This async function is run when someone clicks on the login page's login button
        private async Task OnLoginClicked()
        {
            
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "user email or password cannot be empty", "OK");
                return;
            }
            
            try
            {
                var user = await _userService.LoginAsync(email, Password);

                if (user != null)
                {
                    // Save the token or user details if necessary
                    if (!string.IsNullOrEmpty(user.Token))
                    {
                        await SecureStorage.SetAsync("auth_token", user.Token);
                    }
                    await Shell.Current.GoToAsync("//Dashboard");
                    // navigate to dashboard
                }
                else
                {
                    if (!string.IsNullOrEmpty(user.Token))
                    {
                        await Application.Current.MainPage.DisplayAlert("Login Failed", "Token was not set.", "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Login Failed", "Invalid user email or password.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions here like network errors, timeouts, etc.
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while logging in: " + ex.Message, "OK");
            }
        }
    }
}