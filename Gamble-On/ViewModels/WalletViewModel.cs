using Gamble_On.Services;
using System.Windows.Input;
using Gamble_On.Views.Modals;

namespace Gamble_On.ViewModels
{
    public class WalletViewModel : ViewModelBase
    {
        private readonly IWalletService _walletService;
        private float _amount;
        public ICommand ShowDepositPopupCommand { get; private set; }
        public ICommand ShowWithdravelPopupCommand { get; private set; }
        public ICommand ShowTransactionsPopupCommand { get; private set; }
        public ICommand ShowWalletBettingHistoryPopupPopupCommand { get; private set; }
        public WalletViewModel(IWalletService walletService)
        {
            _walletService = walletService;
            ShowDepositPopupCommand = new Command(async () => await ExecuteShowDepositPopup());
            ShowWithdravelPopupCommand = new Command(async () => await ExecuteShowWithdravelPopup());
            ShowTransactionsPopupCommand = new Command(async () => await ExecuteShowWalletTransactionHistoryPopup());
            ShowWalletBettingHistoryPopupPopupCommand = new Command(async () => await ExecuteShowWalletBettingHistoryPopup());
            MessagingCenter.Subscribe<DepositPopupViewModel, float>(this, "DepositUpdated", OnDepositUpdated);
            MessagingCenter.Subscribe<WithdrawPopupViewModel, float>(this, "WithdrawUpdated", OnWithdrawUpdated);

            LoadWalletData();
        }

        private async Task ExecuteShowDepositPopup()
        {
            var depositViewModel  = App.Current.MainPage.Handler.MauiContext.Services.GetService<DepositPopupViewModel>();
            var depositPage = new DepositPopupPage(depositViewModel);
            await Shell.Current.Navigation.PushModalAsync(depositPage);
        }
        private async Task ExecuteShowWithdravelPopup()
        {
            var withdrawViewModel = App.Current.MainPage.Handler.MauiContext.Services.GetService<WithdrawPopupViewModel>();
            var withdrawPage = new WithdrawPopupPage(withdrawViewModel);
            await Shell.Current.Navigation.PushModalAsync(withdrawPage);
        }

        private async Task ExecuteShowWalletBettingHistoryPopup()
        {
            var bettingHistoryViewModel = App.Current.MainPage.Handler.MauiContext.Services.GetService<WalletBettingHistoryViewModel>();
            var bettingHistoryPage = new WalletBettingHistory(bettingHistoryViewModel);
            await Shell.Current.Navigation.PushModalAsync(bettingHistoryPage);
        }
        private async Task ExecuteShowWalletTransactionHistoryPopup()
        {
            var transactionHistoryViewModel = App.Current.MainPage.Handler.MauiContext.Services.GetService<WalletTransactionHistoryViewModel>();
            var transactionHistoryPage = new WalletTransactionHistory(transactionHistoryViewModel);
            await Shell.Current.Navigation.PushModalAsync(transactionHistoryPage);
        }

        public float Amount
        {
            get => _amount;
            set => Set(ref _amount, value);
        }

        private async void LoadWalletData()
        {
            try
            {
                // Retrieve the user ID stored after login from SecureStorage.
                var userIdStr = await SecureStorage.GetAsync("user_id");
                if (int.TryParse(userIdStr, out int userId) && userId > 0)
                {
                    var wallet = await _walletService.GetWalletByUserIdAsync(userId);
                    if (wallet != null)
                    {
                        Amount = wallet.amount;
                    }
                }
                else
                {
                    // Handle situations where userID isn't available or is incorrect.
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions related to SecureStorage.
            }
        }
        private void OnDepositUpdated(DepositPopupViewModel sender, float depositAmount)
        {
            Amount += depositAmount;
        }
        private void OnWithdrawUpdated(WithdrawPopupViewModel sender, float depositAmount)
        {
            Amount -= depositAmount;
        }

        public void Dispose()
        {
            MessagingCenter.Unsubscribe<DepositPopupViewModel, float>(this, "DepositUpdated");
            MessagingCenter.Unsubscribe<WithdrawPopupViewModel, float>(this, "WithdrawUpdated");

        }
    }
}
