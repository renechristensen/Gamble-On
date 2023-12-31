﻿using Gamble_On.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gamble_On.Services
{
    public interface IGameService
    {
        Task<List<BettingGame>> GetAllBettingGamesAsync();
        Task<Character> GetCharacterByIdAsync(int gameId);
    }
}
