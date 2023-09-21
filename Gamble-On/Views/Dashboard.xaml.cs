namespace Gamble_On.Views;

using Gamble_On.ViewModels;
using Gamble_On;


public partial class Dashboard : ContentPage
{
	public Dashboard(MainDashboardViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}