using Gamble_On.Services;
using Gamble_On.ViewModels;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace Gamble_On.ViewModels;
public class DepositPopupViewModel : ViewModelBase
{
    private readonly IWalletService _walletService;
    private float _depositAmount;

    public ICommand CancelCommand { get; set; }
    public ICommand ContinueCommand { get; set; }

    public DepositPopupViewModel(IWalletService walletService)
    {
        _walletService = walletService;
        CancelCommand = new Command(async () => await ClosePopup());
        ContinueCommand = new Command(async () => await ProcessDeposit());
    }

    public float DepositAmount
    {
        get => _depositAmount;
        set => Set(ref _depositAmount, value);
    }

    private async Task ClosePopup()
    {
        // Close the popup
        await Shell.Current.Navigation.PopModalAsync();
    }

    private async Task ProcessDeposit()
    {
        // Handle the deposit logic here
        if (_depositAmount > 0)
        {
            var userIdStr = await SecureStorage.GetAsync("user_id");
            if (int.TryParse(userIdStr, out int userId))
            {
                var success = await _walletService.DepositAsync(userId, _depositAmount);
                if (success)
                {
                    // Notify the user that the deposit was s

                    MessagingCenter.Send(this, "DepositUpdated", _depositAmount);
                }
                else
                {
                    // Notify the user about any errors
                }
            }
        }
        else
        {
            // Notify the user that the deposit amount should be positive
        }

        await ClosePopup();
    }
}

