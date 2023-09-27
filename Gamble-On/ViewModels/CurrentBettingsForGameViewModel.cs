using Gamble_On.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using Gamble_On.Models;
using Gamble_On.Views.Modals;

namespace Gamble_On.ViewModels
{
    public class CurrentBettingsForGameViewModel : ViewModelBase
    {
        private readonly IGameService _gameService;
        private int _gameId;


        public ICommand ReturnCommand { get; }
        public ICommand NavigateToBettingPageCommand { get; }

        public int GameId
        {
            get => _gameId;
            set
            {
                if (_gameId != value)
                {
                    _gameId = value;
                    OnPropertyChanged();
                    LoadBettingGames();
                }
            }
        }

        public ObservableCollection<BettingGame> BettingGamesForGameId { get; private set; } = new ObservableCollection<BettingGame>();

        public CurrentBettingsForGameViewModel(IGameService gameService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            ReturnCommand = new Command(Return);
            NavigateToBettingPageCommand = new Command<int>(async (gameId) => await ExecuteShowPopup<BettingViewModel, BettingPage>(gameId));

            // Subscribe to the message
            MessagingCenter.Subscribe<MainDashboardViewModel, int>(this, "OpenCurrentBettingsForGame", (sender, receivedGameId) =>
            {
                GameId = receivedGameId;
            });
        }

        private async void Return()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        public async void LoadBettingGames()
        {
            BettingGamesForGameId.Clear();
            try
            {
                List<BettingGame> bettingGamesLocal = await _gameService.GetAllBettingGamesAsync();
                var filteredBettingGames = bettingGamesLocal.Where(bg => bg.GameId == _gameId);
                foreach (var bettingGame in filteredBettingGames)
                {
                    BettingGamesForGameId.Add(bettingGame);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"An error occurred while loading betting games: {ex.Message}", "OK");
            }
        }

        private async Task ExecuteShowPopup<TViewModel, TPage>(int bettingGameId)
        {
            var viewModel = App.Current.MainPage.Handler.MauiContext.Services.GetService<TViewModel>();
            MessagingCenter.Send(this, "OpenBettingPage", bettingGameId); // Send the message
            var page = Activator.CreateInstance(typeof(TPage), viewModel);
            await Shell.Current.Navigation.PushModalAsync(page as Page);
        }
    }
}
