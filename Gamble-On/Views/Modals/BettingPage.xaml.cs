using Gamble_On.ViewModels;

namespace Gamble_On.Views.Modals;

public partial class BettingPage : ContentPage
{
	public BettingPage(BettingViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}