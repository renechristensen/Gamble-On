namespace Gamble_On;
using Gamble_On.ViewModels;

public partial class MainPage : ContentPage
{
    private readonly DoNothingViewModel _viewModel;
    public MainPage(DoNothingViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; 
    }
}