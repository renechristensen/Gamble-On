using Gamble_On.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Services
{
    public interface IBettingService
    {
        Task<bool> PostBettingHistoryAsync(BettingHistory bettingHistory);
        Task<List<BettingHistory>> GetBettingHistoryByUserIdAsync(int userId);
        Task<List<BettingHistoryAlter>> GetBettingHistoryAlterByUserIdAsync(int userId);


    }
}
