using Gamble_On.ViewModels;

namespace Gamble_On.Views.Modals;

public partial class DepositPopupPage : ContentPage
{
	public DepositPopupPage(DepositPopupViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}