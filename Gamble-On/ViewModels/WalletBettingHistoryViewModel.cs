using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Gamble_On.Models;
using Gamble_On.Services;
using Microsoft.Maui.Controls;

namespace Gamble_On.ViewModels
{
    public class WalletBettingHistoryViewModel : ViewModelBase
    {
        private readonly IWalletService _walletService;
        private ObservableCollection<BettingHistory> _bettingHistories;

        public ICommand ClosePopupCommand { get; set; }
        public ICommand LoadAllBettingHistoryCommand { get; set; }  // New command

        public WalletBettingHistoryViewModel(IWalletService walletService)
        {
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
            ClosePopupCommand = new Command(async () => await ClosePopup());
            LoadAllBettingHistoryCommand = new Command(async () => await LoadBettingHistory(false));  // New command initialization
            LoadBettingHistory();
        }

        public ObservableCollection<BettingHistory> BettingHistories
        {
            get => _bettingHistories;
            set => Set(ref _bettingHistories, value);
        }

        private async Task ClosePopup()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        private async Task LoadBettingHistory(bool initialLoad = true)
        {
            try
            {
                var userIdStr = await SecureStorage.GetAsync("user_id");
                if (int.TryParse(userIdStr, out int userId) && userId > 0)
                {
                    List<BettingHistory> histories = await _walletService.GetBettingHistoryByUserIdAsync(userId);
                    if (histories != null)
                    {
                        foreach (BettingHistory history in histories)
                        {
                            if (history.outcome == null)
                            {
                                history.gameResultSoFar = "Spillet er forsat uafgjort";
                            }
                            else if ((bool)history.outcome) 
                            {
                                history.gameResultSoFar = "Du vandt!!";
                            }
                            else
                            {
                                history.gameResultSoFar = "Du tabte";
                            }
                        }
                        if (initialLoad)
                        {
                            BettingHistories = new ObservableCollection<BettingHistory>(histories.OrderByDescending(h => h.createdTime).Take(10));
                        }
                        else
                        {
                            BettingHistories = new ObservableCollection<BettingHistory>(histories.OrderByDescending(h => h.createdTime));
                        }
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "User ID is missing or incorrect.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred while loading betting history: {ex.Message}", "OK");
            }
        }
    }
}
