using Gamble_On;
using static Gamble_On.Views.RegisterPage;
using System.Text.Json;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Gamble_On.Models;

using Gamble_On.ViewModels;



namespace Gamble_On.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(UserRegisterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}