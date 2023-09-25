using Gamble_On.Models;
using Newtonsoft.Json;

namespace Gamble_On.Services
{
    public class GameService : BaseService, IGameService
    {
        // This URL is relative to the base address set in the HttpClient
        private const string Endpoint = "/BettingGame/GetCurrentBettingGames";

        public GameService(HttpClient httpClient, IAuthorizationService authorizationService)
            : base(httpClient, authorizationService)
        { }

        public async Task<List<BettingGame>> GetAllBettingGamesAsync()
        {
            var response = await ExecuteHttpRequestAsync(() => _httpClient.GetAsync(Endpoint));

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                try
                {
                    return JsonConvert.DeserializeObject<List<BettingGame>>(json);
                }
                catch (JsonException)
                {
                    throw new Exception("Failed to deserialize the response from the server.");
                }
            }
            else
            {
                throw new Exception($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
    }
}

