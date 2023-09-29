using Gamble_On.Services;
using Gamble_On.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace Gamble_On.ViewModels
{
    public partial class BetsViewModel : ViewModelBase
    {
        private readonly IBettingService _bettingService;
        private ObservableCollection<BettingHistoryAlter> _bettingHistories;
        private ObservableCollection<BettingHistoryAlter> _completedBets;
        private ObservableCollection<BettingHistoryAlter> _ongoingBets;

        public BetsViewModel(IBettingService bettingService)
        {
            _bettingService = bettingService ?? throw new ArgumentNullException(nameof(bettingService));
            LoadDataCommand = new RelayCommand(Appearing);
            //LoadData();
        }

        public ObservableCollection<BettingHistoryAlter> BettingHistories
        {
            get => _bettingHistories;
            set => Set(ref _bettingHistories, value);
        }
        public ObservableCollection<BettingHistoryAlter> CompletedBets
        {
            get => _completedBets;
            set => Set(ref _completedBets, value);
        }

        public ObservableCollection<BettingHistoryAlter> OngoingBets
        {
            get => _ongoingBets;
            set => Set(ref _ongoingBets, value);
        }

        public ICommand LoadDataCommand { get; }

        public async void LoadData()
        {
            try
            {
                var userIdStr = await SecureStorage.GetAsync("user_id");
                if (int.TryParse(userIdStr, out int userId) && userId > 0)
                {
                    var bettingHistories = await _bettingService.GetBettingHistoryAlterByUserIdAsync(userId);
                    if (bettingHistories != null)
                    {
                        BettingHistories = new ObservableCollection<BettingHistoryAlter>(bettingHistories);
                        OngoingBets = new ObservableCollection<BettingHistoryAlter>();
                        CompletedBets = new ObservableCollection<BettingHistoryAlter>();

                        foreach (BettingHistoryAlter bet in BettingHistories)
                        {
                            // game has not been held
                            if (bet.outcome == null)
                            {
                                OngoingBets.Add(bet);
                            }
                            else
                            {
                                CompletedBets.Add(bet);
                            }
                        }
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
            LoadData();
        }
    }
}

