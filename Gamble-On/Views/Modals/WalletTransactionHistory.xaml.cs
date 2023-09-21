using Gamble_On.ViewModels;

namespace Gamble_On.Views.Modals;

public partial class WalletTransactionHistory : ContentPage
{
	public WalletTransactionHistory(WalletTransactionHistoryViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}