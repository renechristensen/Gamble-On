namespace Gamble_On;
using Gamble_On.ViewModels;
using Gamble_On;
using static Gamble_On.MainPage;
using System.Text.Json;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Gamble_On.Models;

public partial class MainPage : ContentPage
{
    public MainPage(UserLoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; 
    }
}