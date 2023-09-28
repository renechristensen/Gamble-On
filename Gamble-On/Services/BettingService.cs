using Gamble_On.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Services
{
    public class BettingService : BaseService, IBettingService
    {
        public BettingService(HttpClient httpClient, IAuthorizationService authorizationService)
            : base(httpClient, authorizationService)
        {
        }

        // use this to get bets. Refactor WalletService later as well as the viewmodels that need this function
        public async Task<List<BettingHistory>> GetBettingHistoryByUserIdAsync(int userId)
        {
            var endpoint = $"/BettingHistory/UserId/{userId}";
            var response = await ExecuteHttpRequestAsync(() => _httpClient.GetAsync(endpoint));
            int number = 3;
            Console.WriteLine(number);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<BettingHistory>>(await response.Content.ReadAsStringAsync());
            }

            throw new Exception($"Failed to get betting history: {response.StatusCode}");
        }

        public async Task<bool> PostBettingHistoryAsync(BettingHistory bettingHistory)
        {
            if (bettingHistory == null)
                throw new ArgumentNullException(nameof(bettingHistory));

            var endpoint = $"/BettingHistory";
            var jsonPayload = JsonConvert.SerializeObject(bettingHistory);
            StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");

            var response = await ExecuteHttpRequestAsync(() => _httpClient.PostAsync(endpoint, content));

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new Exception($"Failed to post betting history: {response.StatusCode}");
        }
    }
}
