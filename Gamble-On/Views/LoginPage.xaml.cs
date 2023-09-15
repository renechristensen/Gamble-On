namespace Gamble_On;
using Gamble_On.ViewModels;
using Gamble_On;
using static Gamble_On.LoginPage;
using System.Text.Json;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Gamble_On.Models;

public partial class LoginPage : ContentPage
{
    public LoginPage(UserLoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; 
    }
}