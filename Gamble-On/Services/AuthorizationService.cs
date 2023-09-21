using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public async Task SetAuthorizationHeader(HttpClient httpClient)
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Token is missing or invalid");
            }
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}
