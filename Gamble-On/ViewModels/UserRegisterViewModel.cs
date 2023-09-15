using Gamble_On.Services;
using System.Windows.Input;
using Gamble_On.Models;
using System.Diagnostics;

namespace Gamble_On.ViewModels
{
    public class UserRegisterViewModel : ViewModelBase
    {
        private string _firstName;
        private string _lastName;
        private string _username;
        private string _password;
        private string _email;
        private int _phoneNumber;
        private string _address;
        private int _postalCode;
        private string _userType;

        private readonly IUserService _userService;

        public ICommand RegisterCommand { get; }
        public UserRegisterViewModel(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            RegisterCommand = new Command(async () => await OnRegisterClicked());
        }

        public string FirstName
        {
            get => _firstName;
            set => Set(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => Set(ref _lastName, value);
        }

        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        public int PhoneNumber
        {
            get => _phoneNumber;
            set => Set(ref _phoneNumber, value);
        }

        public string Address
        {
            get => _address;
            set => Set(ref _address, value);
        }

        public int PostalCode
        {
            get => _postalCode;
            set => Set(ref _postalCode, value);
        }

        public string UserType
        {
            get => _userType;
            set => Set(ref _userType, value);
        }
        private async Task OnRegisterClicked()
        {
            Debug.WriteLine("Lets debug");
            var newUser = new User
            {
                firstName = FirstName,
                lastName = LastName,
                username = Username,
                password = Password,
                email = Email,
                phoneNumber = PhoneNumber,
                Address = new Address
                {
                    address = Address,
                    postalCode = PostalCode
                },
                UserType = UserType
                
            };

            try
            {
                var success = await _userService.RegisterUserAsync(newUser);
                if (success)
                {
                    await Shell.Current.GoToAsync("//LoginPage");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Registration Failed", "Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred during registration: " + ex.Message, "OK");
            }
        }
    }
}