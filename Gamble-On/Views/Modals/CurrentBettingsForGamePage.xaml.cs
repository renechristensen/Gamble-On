using Gamble_On.ViewModels;
namespace Gamble_On.Views.Modals;
public partial class CurrentBettingsForGamePage : ContentPage
{
	public CurrentBettingsForGamePage(CurrentBettingsForGameViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}