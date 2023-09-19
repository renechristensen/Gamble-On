using Gamble_On.Services;
using System.Windows.Input;
using Gamble_On.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
        private DateTime _dateOfBirth;
        private readonly IUserService _userService;




        public ICommand RegisterCommand { get; }
        public UserRegisterViewModel(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            RegisterCommand = new Command(async () => await OnRegisterClicked());
        }


       

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set => Set(ref _dateOfBirth, value);
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

        public string Address1
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
            // First Name, Last Name, Username, Email, Address Validation
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Address1))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "All fields are required.", "OK");
                return;
            }

            // Password Validation
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");
            var hasLowerChar = new Regex(@"[a-z]+");

            if (!(hasNumber.IsMatch(Password) &&
                  hasUpperChar.IsMatch(Password) &&
                  hasMinimum8Chars.IsMatch(Password) &&
                  hasLowerChar.IsMatch(Password)))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Password should be minimum of 8 characters, contain at least one uppercase letter, one lowercase letter, and one number.", "OK");
                return;
            }

            // Email Validation
            var emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"; // Basic regex pattern for email validation.
            if (!Regex.IsMatch(Email, emailPattern))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Please provide a valid email address.", "OK");
                return;
            }

            // Phone Number Validation
            if (PhoneNumber.ToString().Length < 8)
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error", "Phone number should be at least 8 digits.", "OK");
                return;
            }
            Debug.WriteLine("Lets debug");
            var newUser = new User
            {
                firstName = FirstName,
                lastName = LastName,
                username = Username,
                password = Password,
                email = Email,
                phoneNumber = PhoneNumber,
                dateOfBirth = DateOfBirth,
                Address = new Address
                {
                    address1 = Address1,
                    postalCode = PostalCode
                }

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