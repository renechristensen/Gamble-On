using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Services
{
    public interface IAuthorizationService
    {
        Task<string> GetAuthorizationTokenAsync();
    }
}

