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
                title: "Udbetaling",
                message: "Hvor meget vil du gerne have udbetalt?",
                placeholder: "Antal:",
                maxLength: 5, // Example length
                keyboard: Keyboard.Telephone);
            if (string.IsNullOrEmpty(result))
            {
                return;
            }
            if (float.TryParse(result, out float withdrawAmount) && withdrawAmount > 0)
            {
                await ProcessWithdrawal(withdrawAmount);
            }
            else
            {
                await Shell.Current.DisplayAlert("Fejl", "Du har ikke skrevet et lovligt beløb ind, prøv igen.", "OK");
            }
        }
        private async Task ProcessWithdrawal(float withdrawAmount)
        {
            if (withdrawAmount <= 0)
            {
                await Shell.Current.DisplayAlert("Fejl", "Du kan ikke indbetale et negativt beløb.", "OK");
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
                        await Shell.Current.DisplayAlert("Succes", "Udbetalingen er gennemført", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Fejl", "Der var desvaerre en fejl med din udbetaling, kontakt vores service afdeling.", "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Fejl", "Dit bruger id blev ikke fundet, kontakt en administrator.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fejl", $"Der opstod en databasefejl med din udbetaling, kontakt en administrator: {ex.Message}", "OK");
            }
        }
        private async Task ShowDepositPrompt()
        {
            var result = await Shell.Current.DisplayPromptAsync(
                title: "Indbetaling",
                message: "Hvor meget vil du gerne have udbetalt?",
                placeholder: "Antal:",
                maxLength: 5, // Example length
                keyboard: Keyboard.Telephone);
            if (string.IsNullOrEmpty(result))
            {
                return;
            }
            if (float.TryParse(result, out float depositAmount) && depositAmount > 0)
            {
                await ProcessDeposit(depositAmount);
            }
            else
            {
                await Shell.Current.DisplayAlert("Fejl", "Du har ikke skrevet et tal ind", "OK");
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
                        await Shell.Current.DisplayAlert("Succes", "Indbetaling var en succes.", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Fejl", "Indbetalingen fejlede.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Fejl", $"Der er sket en fejl paa databasen: {ex.Message}", "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Fejl", "Der er en fejl med dit login. Logger af", "OK");
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
                    await Application.Current.MainPage.DisplayAlert("Fejl", "Dit bruger Id er blevet afvist.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fejl", "Der er desvaerre sket en fejl mens vi hentede dinne saldo data: " + ex.Message, "OK");
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