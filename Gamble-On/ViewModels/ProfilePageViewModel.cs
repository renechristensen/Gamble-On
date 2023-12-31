﻿using Microsoft.Maui.Controls;
using Gamble_On.Models;
using Gamble_On.Services;
using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace Gamble_On.ViewModels
{
    public partial class ProfilePageViewModel : ViewModelBase
    {
        private readonly IUserService _userService;

        private string _firstName;
        private string _lastName;
        private string _username;
        private string _email;
        private string _address1;
        private string _phoneNumber;
        private string _dateOfBirth;
        private string _postalCode;

        public ProfilePageViewModel(IUserService userService)
        {
            _userService = userService;
            LoadUserProfile();
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

        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        public string Address1
        {
            get => _address1;
            set => Set(ref _address1, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => Set(ref _phoneNumber, value);
        }

        public string DateOfBirth
        {
            get => _dateOfBirth;
            set => Set(ref _dateOfBirth, value);
        }

        public string PostalCode
        {
            get => _postalCode;
            set => Set(ref _postalCode, value);
        }

        private async void LoadUserProfile()
        {
            try
            {
                var userId = await SecureStorage.GetAsync("user_id");
                if (int.TryParse(userId, out int id))
                {
                    var user = await _userService.GetUserByIdAsync(userId);
                    if (user != null)
                    {
                        FirstName = user.firstName;
                        LastName = user.lastName;
                        Username = user.username;
                        Email = user.email;
                        PhoneNumber = user.phoneNumber.ToString();
                        DateOfBirth = user.dateOfBirth.ToString("dd/MM/yyyy");
                        Address1 = user.Address?.address1;
                        PostalCode = user.Address?.postalCode.ToString();
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Fejl", "Vi kunne ikke finde dine brugeroplysninger, kontakt en administrator", "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Fejl", "Vi kunne ikke finde dit bruger id i databasen, kontakt en administrator", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fejl", $"Der opstod desvaerre en database fejl da vi hentede din profil, kontakt en administrator med følgende besked: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        void Appearing()
        {
            try
            {

                LoadUserProfile();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}
