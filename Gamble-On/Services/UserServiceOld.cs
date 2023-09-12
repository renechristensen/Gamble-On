using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Gamble_On.Models;
using Newtonsoft.Json;

namespace Gamble_On.Services
{
    public class UserServiceOld : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserServiceOld(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var loginEndpoint = "login"; // No need to prefix with BaseUrl, it's set during HttpClient configuration

            var jsonPayload = JsonConvert.SerializeObject(new
            {
                Username = username,
                Password = password
            });

            StringContent message = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            try
            {
                // Attempt to send the POST request
                response = await _httpClient.PostAsync(loginEndpoint, message);
            }
            catch (HttpRequestException e)
            {
                // Handle specific network exceptions here if needed
                throw new Exception("Network error occurred", e);
            }
            catch (TaskCanceledException e)
            {
                if (e.CancellationToken.IsCancellationRequested)
                    throw new Exception("Request was cancelled", e);
                else
                    throw new Exception("Request timed out", e);
            }

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(jsonResponse);
                return user;
            }
            else
            {
                throw new Exception("Login failed");
            }
        }
    }
}


