namespace Gamble_On;
using Gamble_On.ViewModels;
using Gamble_On;

public partial class MainPage : ContentPage
{
    public MainPage(UserLoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; 
    }
}