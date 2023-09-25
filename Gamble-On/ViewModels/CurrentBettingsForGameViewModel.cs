using Gamble_On.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using Gamble_On.Models;


namespace Gamble_On.ViewModels
{
    public class CurrentBettingsForGameViewModel : ViewModelBase
    {
        private readonly IGameService _gameService;
        private int _gameId;
        public ICommand ReturnCommand { get; }

        public int GameId
        {
            get => _gameId;
            set
            {
                if (_gameId != value)
                {
                    _gameId = value;
                    OnPropertyChanged();
                    LoadBettingGames(); // Load data when game ID changes
                }
            }
        }
        public ObservableCollection<BettingGame> BettingGamesForGameId { get; private set; } = new ObservableCollection<BettingGame>();

        public CurrentBettingsForGameViewModel(IGameService gameService, int gameId)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            ReturnCommand = new Command(Return);
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

                // Filter the games for the specific gameId
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
    }
}