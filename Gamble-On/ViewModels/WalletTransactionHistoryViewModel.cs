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
        public ICommand LoadAllTransactionsCommand { get; }

        public WalletTransactionHistoryViewModel(IWalletService walletService)
        {
            ClosePopupCommand = new Command(async () => await ClosePopup());
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
            LoadAllTransactionsCommand = new Command(async () => await LoadAllTransactions());
            LoadTransactions();
        }
        private async Task LoadAllTransactions()
        {
            await LoadTransactions(false);
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

        private async Task LoadTransactions(bool initialLoad = true)
        {
            try
            {
                var userIdStr = await SecureStorage.GetAsync("user_id");
                if (int.TryParse(userIdStr, out int userId) && userId > 0)
                {
                    var transactions = await _walletService.GetTransactionsByUserIdAsync(userId);
                    if (transactions != null)
                    {
                        foreach (var transaction in transactions)
                        {
                            transaction.description = transaction.amount < 0 ? "Udbetaling" : "Indbetaling";
                        }

                        if (initialLoad)
                        {
                            Transactions = new ObservableCollection<Transaction>(transactions.OrderByDescending(t => t.actionTime).Take(10));
                        }
                        else
                        {
                            Transactions = new ObservableCollection<Transaction>(transactions.OrderByDescending(t => t.actionTime));
                        }
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