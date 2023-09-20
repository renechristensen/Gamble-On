namespace Gamble_On.Views;

using Gamble_On.ViewModels;
using static Gamble_On.Views.ProfilePage;
public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfilePageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}