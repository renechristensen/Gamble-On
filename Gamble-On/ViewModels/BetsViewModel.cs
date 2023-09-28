using Gamble_On.Services;
using Gamble_On.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Gamble_On.ViewModels
{
    public partial class BetsViewModel : ViewModelBase
    {
        private readonly IBettingService _bettingService;
        private ObservableCollection<BettingHistory> _bettingHistories;

        public BetsViewModel(IBettingService bettingService)
        {
            _bettingService = bettingService ?? throw new ArgumentNullException(nameof(bettingService));
            LoadDataCommand = new RelayCommand(Appearing);
            LoadData();
        }

        public ObservableCollection<BettingHistory> BettingHistories
        {
            get => _bettingHistories;
            set => Set(ref _bettingHistories, value);
        }

        public ICommand LoadDataCommand { get; }

        public async void LoadData()
        {
            try
            {
                var userIdStr = await SecureStorage.GetAsync("user_id");
                if (int.TryParse(userIdStr, out int userId) && userId > 0)
                {
                    var bettingHistories = await _bettingService.GetBettingHistoryByUserIdAsync(userId);
                    if (bettingHistories != null)
                    {
                        BettingHistories = new ObservableCollection<BettingHistory>(bettingHistories);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "User ID is not available or incorrect.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"An error occurred while loading betting histories: {ex.Message}", "OK");
            }
        }

        private async void Appearing()
        {
            //LoadData();
        }
    }
}

