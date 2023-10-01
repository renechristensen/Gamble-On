using Gamble_On.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Services
{
    public class WalletService : BaseService, IWalletService
    {
        public WalletService(HttpClient httpClient, IAuthorizationService authorizationService)
            : base(httpClient, authorizationService)
        {
        }

        public async Task<Wallet> GetWalletByUserIdAsync(int userId)
        {
            var endpoint = $"/Wallet/UserId/{userId}";
            var response = await ExecuteHttpRequestAsync(() => _httpClient.GetAsync(endpoint));

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Wallet>(await response.Content.ReadAsStringAsync());
            }

            throw new Exception($"Failed to get wallet: {response.StatusCode}");
        }

        public async Task<bool> DepositAsync(int userId, float amount)
        {
            return await PostTransactionAsync(userId, Math.Abs(amount)); // Ensure amount is positive
        }

        public async Task<bool> WithdrawAsync(int userId, float amount)
        {
            return await PostTransactionAsync(userId, -Math.Abs(amount)); // Ensure amount is negative
        }

        private async Task<bool> PostTransactionAsync(int userId, float amount)
        {
            Wallet wallet = await GetWalletByUserIdAsync(userId);

            if (wallet == null)
                return false;

            var transactionEndpoint = $"/Transaction";
            var transaction = new Transaction
            {
                walletId = wallet.id,
                amount = amount,
                actionTime = DateTime.Now
            };

            var jsonPayload = JsonConvert.SerializeObject(transaction);
            StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");

            var response = await ExecuteHttpRequestAsync(() => _httpClient.PostAsync(transactionEndpoint, content));
            var count = 3;
            return response.IsSuccessStatusCode;
        }

        public async Task<List<BettingHistory>> GetBettingHistoryByUserIdAsync(int userId)
        {
            var endpoint = $"/BettingHistory/UserId/{userId}";
            var response = await ExecuteHttpRequestAsync(() => _httpClient.GetAsync(endpoint));

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<BettingHistory>>(await response.Content.ReadAsStringAsync());
            }

            throw new Exception($"Failed to get betting history: {response.StatusCode}");
        }

        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId)
        {
            var endpoint = $"/Transaction/UserId/{userId}";
            var response = await ExecuteHttpRequestAsync(() => _httpClient.GetAsync(endpoint));

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<Transaction>>(await response.Content.ReadAsStringAsync());
            }

            throw new Exception($"Failed to get transactions: {response.StatusCode}");
        }
    }
}
