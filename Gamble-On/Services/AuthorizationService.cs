using System;
using System.Threading.Tasks;

namespace Gamble_On.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public async Task<string> GetAuthorizationTokenAsync()
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Token is missing or invalid");
            }
            return token;
        }
    }
}