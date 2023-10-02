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
                    await Shell.Current.DisplayAlert("Fejl", "Vi kunne ikke finde det valgte spil, kontakt en administrator.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fejl", $"Der opstod en database fejl da vi loadeded spillet, vaer venlig at kontakte en administrator med følgende besked: {ex.Message}", "OK");
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
                            await Shell.Current.DisplayAlert("Manglende midler", "Du har desvaerre for lav en saldo til at lave dette sats.", "OK");
                            return;
                        }
                        else
                        {
                            bool withdrawSuccess = await _walletService.WithdrawAsync(userId, amount);
                            if (!withdrawSuccess)
                            {
                                await Shell.Current.DisplayAlert("Fejl", "Der har desvaerre vaeret en databasefejl der gør at din udbetaling er fejlet, kontakt en administrator", "OK");
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
                            await Shell.Current.DisplayAlert("Sats placeret", $"Du har satset {amount} paa {selectedCharacter.name}!", "OK");
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
                            await Shell.Current.DisplayAlert("Fejl", "Der var en fejl med dit sats, kontakt en administrator.", "OK");
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Fejl", "Vi kunne ikke faa fat i dine saldo oplysninger, kontakt en administrator", "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Fejl", "Dit bruger id er desvaerre ikke tilgaengeligt, kontakt en administrator", "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Forkert indput", "Vaer venlig at skrive et tal ind i feltet", "OK");
            }
        }
    }
}