using Gamble_On.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Services
{
    public interface IUserService
    {
        Task<String> LoginAsync(string username, string password);
        Task<bool>RegisterUserAsync(User newUser);
        Task<User> GetUserByIdAsync(string userId);

    }
}

