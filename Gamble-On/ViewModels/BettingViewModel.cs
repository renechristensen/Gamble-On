using Gamble_On.Models;
using Gamble_On.Services;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gamble_On.ViewModels
{
    public class BettingViewModel : ViewModelBase
    {
        private readonly IBettingService _bettingService;
        private readonly IWalletService _walletService;
        private readonly IGameService _gameService;
        private int _id;
        public ObservableCollection<Character> CharactersForGame { get; private set; } = new ObservableCollection<Character>();

        public ICommand ReturnCommand { get; }
        public ICommand ConfirmBetCommand { get; }

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                    LoadData(); // Load data when id changes
                }
            }
        }

        public BettingViewModel(IBettingService bettingService, IWalletService walletService, IGameService gameService)
        {
            _bettingService = bettingService ?? throw new ArgumentNullException(nameof(bettingService));
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));

            ReturnCommand = new Command(Return);
            ConfirmBetCommand = new Command<Character>(async (selectedCharacter) => await ConfirmBetAsync(selectedCharacter));

            MessagingCenter.Subscribe<CurrentBettingsForGameViewModel, int>(this, "OpenBettingPage", (sender, receivedBettingGameId) =>
            {
                Id = receivedBettingGameId;
            });
        }

        private async void Return()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        public async void LoadData()
        {
            try
            {
                var allBettingGames = await _gameService.GetAllBettingGamesAsync();
                var gameDetails = allBettingGames.FirstOrDefault(bg => bg.Id == Id);

                if (gameDetails != null && gameDetails.Game != null)
                {
                    CharactersForGame.Clear();
                    foreach (var character in gameDetails.Game.characters)
                    {
                        CharactersForGame.Add(character);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "The desired game details were not found.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"An error occurred while loading game details: {ex.Message}", "OK");
            }
        }

        private async Task ConfirmBetAsync(Character selectedCharacter)
        {
            var betAmount = await Shell.Current.DisplayPromptAsync(
                title: "Place Bet",
                message: $"How much would you like to bet on {selectedCharacter.name}?",
                placeholder: "Enter amount",
                maxLength: 5, // Example length
                keyboard: Keyboard.Telephone);

            // Check if betAmount is null or empty, this will be true if the user has hit cancel
            if (string.IsNullOrEmpty(betAmount))
            {
                return;
            }

            if (float.TryParse(betAmount, out float amount) && amount > 0)
            {
                var userIdStr = await SecureStorage.GetAsync("user_id");
                if (int.TryParse(userIdStr, out int userId) && userId > 0)
                {
                    var wallet = await _walletService.GetWalletByUserIdAsync(userId);

                    if (wallet != null)
                    {
                        if (amount > wallet.amount)
                        {
                            await Shell.Current.DisplayAlert("Insufficient Funds", "You don't have enough balance in your wallet to place this bet.", "OK");
                            return;
                        }
                        else
                        {
                            bool withdrawSuccess = await _walletService.WithdrawAsync(userId, amount);
                            if (!withdrawSuccess)
                            {
                                await Shell.Current.DisplayAlert("Error", "Failed to withdraw amount from wallet.", "OK");
                                return;
                            }
                        }

                        BettingHistory bettingHistory = new BettingHistory
                        {
                            walletID = wallet.id,
                            bettingAmount = amount,
                            bettingGameId = Id,
                            createdTime = DateTime.Now,
                            bettingCharacterId = selectedCharacter.id,
                            outcome = null,
                            bettingResult = null


                        };

                        bool isSuccess = await _bettingService.PostBettingHistoryAsync(bettingHistory);
                        if (isSuccess)
                        {
                            await Shell.Current.DisplayAlert("Bet Placed", $"You bet {amount} on {selectedCharacter.name}!", "OK");
                            if (Shell.Current.Navigation.ModalStack.Count > 0)
                            {
                                await Shell.Current.Navigation.PopModalAsync();
                            }
                            if (Shell.Current.Navigation.ModalStack.Count > 0)
                            {
                                await Shell.Current.Navigation.PopModalAsync();
                            }
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Error", "There was an issue placing your bet. Please try again.", "OK");
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Failed to fetch wallet data.", "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "User ID is not available or incorrect.", "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Invalid Input", "Please enter a valid bet amount.", "OK");
            }
        }
    }
}