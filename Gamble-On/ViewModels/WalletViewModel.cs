using Gamble_On.Services;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Gamble_On.Views.Modals;
using CommunityToolkit.Mvvm.Input;

namespace Gamble_On.ViewModels
{
    public partial class WalletViewModel : ViewModelBase
    {
        private readonly IWalletService _walletService;
        private float _amount;

        public ICommand ShowDepositPopupCommand { get; }
        public ICommand ShowWithdravelPopupCommand { get; }
        public ICommand ShowTransactionsPopupCommand { get; }
        public ICommand ShowWalletBettingHistoryPopupPopupCommand { get; }

        public WalletViewModel(IWalletService walletService)
        {
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));

            ShowDepositPopupCommand = new Command(async () => await ExecuteShowPopup<DepositPopupViewModel, DepositPopupPage>());
            ShowWithdravelPopupCommand = new Command(async () => await ExecuteShowPopup<WithdrawPopupViewModel, WithdrawPopupPage>());
            ShowTransactionsPopupCommand = new Command(async () => await ExecuteShowPopup<WalletTransactionHistoryViewModel, WalletTransactionHistory>());
            ShowWalletBettingHistoryPopupPopupCommand = new Command(async () => await ExecuteShowPopup<WalletBettingHistoryViewModel, WalletBettingHistory>());
            LoadWalletData();
        }

        public float Amount
        {
            get => _amount;
            set => Set(ref _amount, value);
        }

        private async Task ExecuteShowPopup<TViewModel, TPage>()
        {
            var viewModel = App.Current.MainPage.Handler.MauiContext.Services.GetService<TViewModel>();
            var page = Activator.CreateInstance(typeof(TPage), viewModel);
            await Shell.Current.Navigation.PushModalAsync(page as Page);
        }

        private async void LoadWalletData()
        {
            try
            {
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
                    await Application.Current.MainPage.DisplayAlert("Error", "User ID is not available or incorrect.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while loading wallet data: " + ex.Message, "OK");
            }
        }

        [RelayCommand]
        void Appearing()
        {
            try
            {

                LoadWalletData();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}