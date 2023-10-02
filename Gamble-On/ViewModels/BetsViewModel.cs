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
        private readonly IWalletService _walletService;
        private ObservableCollection<BettingHistoryAlter> _bettingHistories;
        private ObservableCollection<BettingHistoryAlter> _completedBets;
        private ObservableCollection<BettingHistoryAlter> _ongoingBets;

        public ICommand RemoveBetCommand { get; set; }
        public BetsViewModel(IBettingService bettingService, IWalletService walletService)
        {
            _bettingService = bettingService ?? throw new ArgumentNullException(nameof(bettingService));
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
            LoadDataCommand = new RelayCommand(Appearing);
            RemoveBetCommand = new Command<BettingHistoryAlter>(async (bet) => await RemoveBetAsync(bet));
            //LoadData();
        }
        private async Task RemoveBetAsync(BettingHistoryAlter betToRemove)
        {
            try
            {
                var isRemoved = await _bettingService.RemoveBetAsync(betToRemove.id);
                if (isRemoved)
                {
                    OngoingBets.Remove(betToRemove);  // Remove from local collection as well.

                    // Deposit the betting amount back to user's wallet
                    var userIdStr = await SecureStorage.GetAsync("user_id");
                    if (int.TryParse(userIdStr, out int userId) && userId > 0)
                    {
                        await _walletService.DepositAsync(userId, betToRemove.bettingAmount);
                    }

                    await Shell.Current.DisplayAlert("Succes", "Sats er fjernet og dine penge er blevet returneret.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fejl", $"Der skete en fejl da vi forsøgte at fjerne dit sat, kontakt en administrator med følgende besked: {ex.Message}", "OK");
            }
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
                    await Shell.Current.DisplayAlert("Fejl", "Dit bruger id blev ikke fundet, kontakt en administrator", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fejl", $"Der skete en database fejl da vi forsøgte at indlaese dine sats, kontakt en administrator med følgende besked: {ex.Message}", "OK");
            }
        }

        private async void Appearing()
        {
            LoadData();
        }
    }
}