using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gamble_On.Models;
using Newtonsoft.Json;

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
            Wallet wallet = await GetWalletByUserIdAsync(userId);

            if (wallet == null)
                return false;

            wallet.amount += amount;
            var walletUpdateEndpoint = $"/Wallet/{wallet.id}";
            var jsonPayload = JsonConvert.SerializeObject(wallet);
            StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");

            var response = await ExecuteHttpRequestAsync(() => _httpClient.PutAsync(walletUpdateEndpoint, content));

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> WithdrawAsync(int userId, float amount)
        {
            Wallet wallet = await GetWalletByUserIdAsync(userId);

            if (wallet == null)
                return false;

            wallet.amount -= amount;
            var walletUpdateEndpoint = $"/Wallet/{wallet.id}";
            var jsonPayload = JsonConvert.SerializeObject(wallet);
            StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");

            var response = await ExecuteHttpRequestAsync(() => _httpClient.PutAsync(walletUpdateEndpoint, content));

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
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