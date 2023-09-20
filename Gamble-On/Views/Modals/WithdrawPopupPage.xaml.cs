using Gamble_On.ViewModels;

namespace Gamble_On.Views.Modals;

public partial class WithdrawPopupPage : ContentPage
{
	public WithdrawPopupPage(WithdrawPopupViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}