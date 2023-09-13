using System.Windows.Input;
using Microsoft.Maui.Controls;  // For Command

namespace Gamble_On.ViewModels
{
    public class MainDashboardViewModel : ViewModelBase  // Assuming you have a ViewModelBase for common functionalities
    {
        // Navigation commands for the double bar
        public ICommand NavigateToRufusCommand { get; private set; }
        public ICommand NavigateToDRCommand { get; private set; }

        // Other properties and commands for the dashboard can go here
        // For example, user details, statistics, bets, games, etc.

        public MainDashboardViewModel()
        {
            // Initialize commands
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            NavigateToRufusCommand = new Command(NavigateToRufus);
            NavigateToDRCommand = new Command(NavigateToDR);
        }

        private void NavigateToRufus()
        {
            // Navigation logic to Rufus
        }

        private async void NavigateToDR()
        {
                try
                {
                    await Launcher.OpenAsync(new Uri("https://www.dr.dk/"));
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that might occur while launching the URL
                    await Application.Current.MainPage.DisplayAlert("Error", "The page is not available right now, try again later", "OK");
                }
            
        }
    }
}
