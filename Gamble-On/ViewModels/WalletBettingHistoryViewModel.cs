using Gamble_On.Models;
using Gamble_On.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Gamble_On.ViewModels
{
    public class WalletBettingHistoryViewModel : ViewModelBase
    {
        private readonly IWalletService _walletService;
        private ObservableCollection<BettingHistory> _bettingHistories;
        public ICommand ClosePopupCommand { get; set; }

        public WalletBettingHistoryViewModel(IWalletService walletService)
        {
            ClosePopupCommand = new Command(async () => await ClosePopup());
            _walletService = walletService;
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
                    // Handle situations where userID isn't available or is incorrect.
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions.
            }
        }
    }
}


