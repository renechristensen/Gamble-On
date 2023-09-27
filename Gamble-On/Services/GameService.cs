﻿using Gamble_On.Models;
using Newtonsoft.Json;

namespace Gamble_On.Services
{
    public class GameService : BaseService, IGameService
    {
        // Existing constant
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
        /*
        public async Task<BettingGame> GetGameByIdAsync(int gameId)
        {
            var response = await ExecuteHttpRequestAsync(() => _httpClient.GetAsync($"/BettingGame/{gameId}"));

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                try
                {
                    return JsonConvert.DeserializeObject<BettingGame>(json);
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
        }*/
    }
}

