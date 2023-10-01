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
        public float Amount
        {
            get => _amount;
            set => Set(ref _amount, value);
        }
        public ICommand ShowDepositPromptCommand { get; }
        public ICommand ShowWithdravelPopupCommand { get; }
        public ICommand ShowTransactionsPopupCommand { get; }
        public ICommand ShowWalletBettingHistoryPopupPopupCommand { get; }

        public WalletViewModel(IWalletService walletService)
        {
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
            ShowDepositPromptCommand = new Command(async () => await ShowDepositPrompt());
            ShowWithdravelPopupCommand = new Command(async () => await ShowWithdrawalPrompt());
            ShowTransactionsPopupCommand = new Command(async () => await ExecuteShowPopup<WalletTransactionHistoryViewModel, WalletTransactionHistory>());
            ShowWalletBettingHistoryPopupPopupCommand = new Command(async () => await ExecuteShowPopup<WalletBettingHistoryViewModel, WalletBettingHistory>());
            LoadWalletData();
        }

        private async Task ShowWithdrawalPrompt()
        {
            var result = await Shell.Current.DisplayPromptAsync(
                title: "Withdraw",
                message: "How much would you like to withdraw?",
                placeholder: "Enter amount",
                maxLength: 5, // Example length
                keyboard: Keyboard.Numeric);

            if (float.TryParse(result, out float withdrawAmount) && withdrawAmount > 0)
            {
                await ProcessWithdrawal(withdrawAmount);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Invalid amount entered. Please try again.", "OK");
            }
        }
        private async Task ProcessWithdrawal(float withdrawAmount)
        {
            if (withdrawAmount <= 0)
            {
                await Shell.Current.DisplayAlert("Error", "Withdrawal amount should be positive.", "OK");
                return;
            }

            try
            {
                var userIdStr = await SecureStorage.GetAsync("user_id");
                if (int.TryParse(userIdStr, out int userId))
                {
                    var success = await _walletService.WithdrawAsync(userId, withdrawAmount);
                    if (success)
                    {
                        LoadWalletData();
                        await Shell.Current.DisplayAlert("Success", "Withdrawal was successful.", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Withdrawal failed.", "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "User ID is missing or incorrect.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"An error occurred while processing withdrawal: {ex.Message}", "OK");
            }
        }
        private async Task ShowDepositPrompt()
        {
            var result = await Shell.Current.DisplayPromptAsync(
                title: "Deposit",
                message: "How much would you like to deposit?",
                placeholder: "Enter amount",
                maxLength: 5, // Example length
                keyboard: Keyboard.Numeric);

            if (float.TryParse(result, out float depositAmount) && depositAmount > 0)
            {
                await ProcessDeposit(depositAmount);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Invalid amount entered. Please try again.", "OK");
            }
        }

        private async Task ProcessDeposit(float depositAmount)
        {
            var userIdStr = await SecureStorage.GetAsync("user_id");
            if (int.TryParse(userIdStr, out int userId))
            {
                try
                {
                    var success = await _walletService.DepositAsync(userId, depositAmount);
                    if (success)
                    {
                        LoadWalletData();
                        await Shell.Current.DisplayAlert("Success", "Deposit successfully processed.", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Failure", "Failed to process deposit.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "There has been an error with your login. Logging off", "OK");
            }
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