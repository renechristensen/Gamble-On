using Gamble_On.Services;
using Gamble_On.ViewModels;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace Gamble_On.ViewModels;
public class WithdrawPopupViewModel : ViewModelBase
{
    private readonly IWalletService _walletService;
    private float _withdrawAmount;

    public ICommand CancelCommand { get; set; }
    public ICommand ContinueCommand { get; set; }

    public WithdrawPopupViewModel(IWalletService walletService)
    {
        _walletService = walletService;
        CancelCommand = new Command(async () => await ClosePopup());
        ContinueCommand = new Command(async () => await ProcessDeposit());
    }

    public float WithdrawAmount
    {
        get => _withdrawAmount;
        set => Set(ref _withdrawAmount, value);
    }

    private async Task ClosePopup()
    {
        // Close the popup
        await Shell.Current.Navigation.PopModalAsync();
    }

    private async Task ProcessDeposit()
    {
        // Handle the deposit logic here
        if (_withdrawAmount > 0)
        {
            var userIdStr = await SecureStorage.GetAsync("user_id");
            if (int.TryParse(userIdStr, out int userId))
            {
                var success = await _walletService.WithdrawAsync(userId, _withdrawAmount);
                if (success)
                {
                    // Notify the user that the withdrawel was successful
                    MessagingCenter.Send(this, "WithdrawUpdated", _withdrawAmount);

                }
                else
                {
                    // Notify the user about any errors
                }
            }
        }
        else
        {
            // Notify the user that the wkthdrawel amount should be positive
        }

        await ClosePopup();
    }
}