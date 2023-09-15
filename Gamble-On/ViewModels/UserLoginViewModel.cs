using Gamble_On.Services;
using System.Windows.Input;
using Gamble_On.Views;
using Microsoft.Extensions.DependencyInjection;
using Gamble_On.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

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
                // returns a jsonstring that contains a user class and a token, For now we will just split it up and keep the token.
                var jsonString = await _userService.LoginAsync(email, Password); // returns token
                Root deserializedObject = JsonSerializer.Deserialize<Root>(jsonString);
                string token = deserializedObject.Token;
                User user = deserializedObject.User;
                
                if (token != null)
                {
                    // Save the token or user details if necessary
                    await SecureStorage.SetAsync("auth_token", token);
                    
                    await Shell.Current.GoToAsync("//Dashboard");
                    // navigate to dashboard
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Login Failed", "Invalid user email or password.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions here like network errors, timeouts, etc.
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while logging in: " + ex.Message, "OK");
            }
        }

        // this is a local class used to split an incoming jsonstring into a user class and a token string 
        private class Root
        {
            [JsonPropertyName("login")]
            public User User { get; set; }

            [JsonPropertyName("token")]
            public string Token { get; set; }
        }
    }

}