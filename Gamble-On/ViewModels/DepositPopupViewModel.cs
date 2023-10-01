using Gamble_On.Services;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gamble_On.ViewModels
{
    public class DepositPopupViewModel : ViewModelBase
    {
        private readonly IWalletService _walletService;
        private float _depositAmount;

        public ICommand CancelCommand { get; }
        public ICommand ContinueCommand { get; }

        public float DepositAmount
        {
            get => _depositAmount;
            set => Set(ref _depositAmount, value);
        }

        public DepositPopupViewModel(IWalletService walletService)
        {
            _walletService = walletService;
            CancelCommand = new Command(async () => await ClosePopup());
            ContinueCommand = new Command(async () => await ProcessDeposit());
        }

        private async Task ClosePopup()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        private async Task ProcessDeposit()
        {
            if (_depositAmount > 0)
            {
                var userIdStr = await SecureStorage.GetAsync("user_id");
                if (int.TryParse(userIdStr, out int userId))
                {
                    try
                    {
                        var success = await _walletService.DepositAsync(userId, _depositAmount);
                        if (success)
                        {
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

                    await ClosePopup();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "There has been an error with your login. Logging off", "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Deposit amount should be greater than zero.", "OK");
            }
        }
    }
}