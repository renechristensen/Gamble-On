using Gamble_On.ViewModels;

namespace Gamble_On.Views;

public partial class Bets : ContentPage
{
	public Bets(BetsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}