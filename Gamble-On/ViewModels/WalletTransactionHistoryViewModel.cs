using Gamble_On.Models;
using Gamble_On.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;  // for Command
using System.Threading.Tasks;  // for Task
using System.Linq;  // for OrderBy

namespace Gamble_On.ViewModels
{
    public class WalletTransactionHistoryViewModel : ViewModelBase
    {
        private readonly IWalletService _walletService;
        private ObservableCollection<Transaction> _transactions;

        public ICommand ClosePopupCommand { get; }

        public WalletTransactionHistoryViewModel(IWalletService walletService)
        {
            ClosePopupCommand = new Command(async () => await ClosePopup());
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
            LoadTransactions();
        }

        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set => Set(ref _transactions, value);
        }

        private async Task ClosePopup()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        private async void LoadTransactions()
        {
            try
            {
                var userIdStr = await SecureStorage.GetAsync("user_id");
                if (int.TryParse(userIdStr, out int userId) && userId > 0)
                {
                    var transactions = await _walletService.GetTransactionsByUserIdAsync(userId);
                    if (transactions != null)
                    {
                        Transactions = new ObservableCollection<Transaction>(transactions.OrderBy(t => t.actionTime));
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "User ID is not available or incorrect.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while loading transactions: " + ex.Message, "OK");
            }
        }
    }
}
