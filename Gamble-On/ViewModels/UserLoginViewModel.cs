using Gamble_On.Services;
using System.Windows.Input;
using Gamble_On.Views;
namespace Gamble_On.ViewModels
{
    public class UserLoginViewModel : ViewModelBase
    {
        private string _password;
        private string _username;
        private readonly IUserService _userService;

        public ICommand LoginCommand { get; }

        public UserLoginViewModel(IUserService userService)
        {
            _userService = userService;
            LoginCommand = new Command(async () => await OnLoginClicked());
        }

        // Get&Set properties
        public string Username
        {
            get { return _username; }
            set { Set(ref _username, value); }
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
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Username or password cannot be empty", "OK");
                return;
            }

            try
            {
                var user = await _userService.LoginAsync(Username, Password);

                if (user != null && !string.IsNullOrEmpty(user.Token))
                {
                    // Save the token or user details if necessary
                    // Navigate to another page, for example, the main dashboard or home page
                    await Application.Current.MainPage.Navigation.PushAsync(new Dashboard());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Login Failed", "Invalid username or password.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions here like network errors, timeouts, etc.
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while logging in.", "OK");
            }
        }
    }
}