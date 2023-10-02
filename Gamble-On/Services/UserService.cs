using System;
using System.Text;
using System.Threading.Tasks;
using Gamble_On.Models;
using Newtonsoft.Json;

namespace Gamble_On.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly string LoginEndpoint = "/User/UserLogin";
        private readonly string RegisterEndpoint = "/User/CreateUser";

        public UserService(HttpClient httpClient, IAuthorizationService authorizationService)
            : base(httpClient, authorizationService)
        {
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var jsonPayload = JsonConvert.SerializeObject(new
            {
                email = email,
                password = password
            });

            StringContent message = new(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await ExecuteHttpRequestAsyncWithoutAuthorization(() => _httpClient.PostAsync(LoginEndpoint, message));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Failed to login: {response.StatusCode}");
            }
        }

        public async Task<string> RegisterUserAsync(User userToRegister)
        {
            var jsonPayload = JsonConvert.SerializeObject(userToRegister);
            StringContent message = new(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await ExecuteHttpRequestAsyncWithoutAuthorization(() => _httpClient.PostAsync(RegisterEndpoint, message));

            if (response.IsSuccessStatusCode)
            {
                return "true";
            }
            else
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            var endpoint = $"/User/{userId}";

            HttpResponseMessage response = await ExecuteHttpRequestAsync(() => _httpClient.GetAsync(endpoint));

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                User getUser = JsonConvert.DeserializeObject<User>(jsonResponse);
                int count = 0;
                return getUser;
            }
            else
            {
                throw new Exception($"Failed to fetch user: {response.StatusCode}");
            }
        }
    }
}
