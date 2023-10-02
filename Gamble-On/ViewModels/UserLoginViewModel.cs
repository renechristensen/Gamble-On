using Microsoft.Maui.Controls;
using Gamble_On.Services;
using System.Windows.Input;
using System.Threading.Tasks;
using Gamble_On.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gamble_On.ViewModels
{
    public class UserLoginViewModel : ViewModelBase
    {
        private string _email;
        private string _password;
        private readonly IUserService _userService;

        public ICommand LoginCommand { get; }

        public UserLoginViewModel(IUserService userService)
        {
            _userService = userService;
            LoginCommand = new Command(async () => await OnLoginClicked());
        }

        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        private async Task OnLoginClicked()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await Shell.Current.DisplayAlert("Fejl", "Vaer venlig at udfylde kodeord- og emailfeltet", "OK");
                return;
            }

            try
            {
                var jsonString = await _userService.LoginAsync(Email, Password);
                var deserializedObject = JsonSerializer.Deserialize<Root>(jsonString);

                string token = deserializedObject.Token;
                User user = deserializedObject.User;

                if (!string.IsNullOrEmpty(token))
                {
                    await SecureStorage.SetAsync("auth_token", token);
                    await SecureStorage.SetAsync("user_id", user.id.ToString());
                    await Shell.Current.GoToAsync("//Dashboard");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Login fejlede", "Du har brugt en forkert email eller password", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fejl", $"Der opstod desvaerre en database fejl da du forsøgte at logge ind, kontakt en administrator med følgende besked: {ex.Message}", "OK");
            }
        }

        private class Root
        {
            [JsonPropertyName("login")]
            public User User { get; set; }

            [JsonPropertyName("token")]
            public string Token { get; set; }
        }
    }
}
