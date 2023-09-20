using Gamble_On.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Services
{
    public interface IWalletService
    {
        Task<Wallet> GetWalletByUserIdAsync(int userId);
        Task<bool> DepositAsync(int userId, float amount);
        Task<bool> WithdrawAsync(int userId, float amount);
    }
}
