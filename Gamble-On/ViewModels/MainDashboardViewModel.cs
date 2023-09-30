using Microsoft.Maui.Controls;
using Gamble_On.Services;
using System.Collections.ObjectModel;
using Gamble_On.Models;
using System.Windows.Input;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Gamble_On.Views;
using System;
using Gamble_On.Views.Modals;
using CommunityToolkit.Mvvm.Input;

namespace Gamble_On.ViewModels
{
    public partial class MainDashboardViewModel : ViewModelBase
    {
        private readonly IGameService _gameService;

        public ObservableCollection<BettingGame> BettingGames { get; private set; } = new ObservableCollection<BettingGame>();
        public ObservableCollection<BettingGame> DisplayedGames { get; private set; } = new ObservableCollection<BettingGame>();

        public ICommand ToggleExpandCommand { get; private set; }
        public ICommand OpenBettingGamesModalCommand { get; private set; }

        public MainDashboardViewModel(IGameService gameService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            ToggleExpandCommand = new Command<Game>(ToggleExpand);
            OpenBettingGamesModalCommand = new Command<int>(async (gameId) => await ExecuteShowPopup<CurrentBettingsForGameViewModel, CurrentBettingsForGamePage>(gameId));
        }

        private void ToggleExpand(Game game)
        {
            if (game != null)
            {
                game.IsExpanded = !game.IsExpanded;
                OnPropertyChanged(nameof(DisplayedGames));
            }
        }

        private async void LoadGames()
        {
            BettingGames.Clear();
            DisplayedGames.Clear();
            try
            {
                List<BettingGame> bettingGamesLocal = await _gameService.GetAllBettingGamesAsync();
                HashSet<int> addedGameIds = new HashSet<int>();

                foreach (BettingGame bettingGame in bettingGamesLocal)
                {
                    if (string.IsNullOrEmpty(bettingGame.Game.desc))
                        bettingGame.Game.desc = "There is no default description for this game at the moment.";

                    // Check if image URL is valid, otherwise set default image
                    if (!await IsValidImageUrl(bettingGame.Game.gameImage))
                    {
                        bettingGame.Game.gameImage = "default_image.png";
                    }

                    BettingGames.Add(bettingGame);

                    // If this game's ID hasn't been added to DisplayedGames yet, add it
                    if (addedGameIds.Add(bettingGame.GameId))
                    {
                        bettingGame.GameCount++;
                        DisplayedGames.Add(bettingGame);
                    }
                    else
                    {
                        // find the game in displayedGames using GameId
                        var existingGame = DisplayedGames.FirstOrDefault(game => game.GameId == bettingGame.GameId);
                        if (existingGame != null)
                        {
                            // Add +1 to GameCount property of bettinggame
                            existingGame.GameCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"An error occurred while loading games: {ex.Message}", "OK");
            }
        }

        private async Task<bool> IsValidImageUrl(string url)
        {
            // Placeholder for the moment, until an image server is up this will always be false
            return false;
        }

        
        private async Task ExecuteShowPopup<TViewModel, TPage>(int gameId)
        {
            var viewModel = App.Current.MainPage.Handler.MauiContext.Services.GetService<TViewModel>();
            MessagingCenter.Send(this, "OpenCurrentBettingsForGame", gameId);
            var page = Activator.CreateInstance(typeof(TPage), viewModel);
            await Shell.Current.Navigation.PushModalAsync(page as Page);
        }

        [RelayCommand]
        void Appearing()
        {
            try
            {

                LoadGames();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}

























/* using Gamble_On.Services;
using System.Collections.ObjectModel;
using Gamble_On.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Gamble_On.ViewModels
{
    public class MainDashboardViewModel : ViewModelBase
    {
        
        // This exists for testing with the address service. Its used while working with authentication
         
        private readonly IAddressService _addressService;
        public ObservableCollection<Address> Addresses { get; set; }

        public MainDashboardViewModel(IAddressService addressService)
        {
            _addressService = addressService;
            Addresses = new ObservableCollection<Address>();

            // Use the same pattern as in WalletViewModel
            LoadAddresses();
        }

        private async void LoadAddresses()
        {
            try
            {
                var addresses = await _addressService.GetAllAddressesAsync();
                if (addresses != null)
                {
                    foreach (var address in addresses)
                    {
                        Addresses.Add(address);
                    }
                }
                else
                {
                    // Handle situations where addresses aren't available or something went wrong.
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions.
            }
        }
    }
}
*/

