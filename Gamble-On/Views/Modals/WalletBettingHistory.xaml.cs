namespace Gamble_On.Views.Modals;
using Gamble_On.ViewModels;

public partial class WalletBettingHistory : ContentPage
{
	public WalletBettingHistory(WalletBettingHistoryViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}