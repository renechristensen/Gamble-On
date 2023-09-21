using Gamble_On.Services;
using Gamble_On.ViewModels;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace Gamble_On.ViewModels
{
    public class WithdrawPopupViewModel : ViewModelBase
    {
        private readonly IWalletService _walletService;
        private float _withdrawAmount;

        public ICommand CancelCommand { get; }
        public ICommand ContinueCommand { get; }

        public WithdrawPopupViewModel(IWalletService walletService)
        {
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
            CancelCommand = new Command(async () => await ClosePopup());
            ContinueCommand = new Command(async () => await ProcessWithdrawal());
        }

        public float WithdrawAmount
        {
            get => _withdrawAmount;
            set => Set(ref _withdrawAmount, value);
        }

        private async Task ClosePopup()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        private async Task ProcessWithdrawal()
        {
            if (_withdrawAmount <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Withdrawal amount should be positive.", "OK");
                return;
            }

            try
            {
                var userIdStr = await SecureStorage.GetAsync("user_id");
                if (int.TryParse(userIdStr, out int userId))
                {
                    var success = await _walletService.WithdrawAsync(userId, _withdrawAmount);
                    if (success)
                    {
                        MessagingCenter.Send(this, "WithdrawUpdated", _withdrawAmount);
                        await Application.Current.MainPage.DisplayAlert("Success", "Withdrawal was successful.", "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Withdrawal failed.", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "User ID is missing or incorrect.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while processing withdrawal: " + ex.Message, "OK");
            }
            finally
            {
                await ClosePopup();
            }
        }
    }
}
