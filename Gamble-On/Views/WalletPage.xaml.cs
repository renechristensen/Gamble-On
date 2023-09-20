using Gamble_On.ViewModels;
using Gamble_On;
using static Gamble_On.Views.WalletPage;
using System.Text.Json;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Gamble_On.Views;


public partial class WalletPage : ContentPage
{
	public WalletPage(WalletViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}