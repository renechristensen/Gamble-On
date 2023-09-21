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

        public WalletBettingHistoryViewModel(IWalletService walletService)
        {
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
            ClosePopupCommand = new Command(async () => await ClosePopup());
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

        private async void LoadBettingHistory()
        {
            try
            {
                var userIdStr = await SecureStorage.GetAsync("user_id");
                if (int.TryParse(userIdStr, out int userId) && userId > 0)
                {
                    var histories = await _walletService.GetBettingHistoryByUserIdAsync(userId);
                    if (histories != null)
                    {
                        BettingHistories = new ObservableCollection<BettingHistory>(histories.OrderBy(h => h.createdTime));
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