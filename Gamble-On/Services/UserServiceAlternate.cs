using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Gamble_On.Models;
using Newtonsoft.Json;
/*
namespace Gamble_On.Services
{
    public class UserServiceAlternate : IUserService
    {
        private static readonly string BaseUrl = "https://localhost:7138/";
        // Declaring as static readonly is just an optimization to ensure it's only set once for all instances.
        // If your URL can change over time, adjust this accordingly.

        public UserServiceAlternate()
        {
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };
            return httpClient;
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            using (var httpClient = CreateHttpClient())
            {
                var loginEndpoint = "login";

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
                    response = await httpClient.PostAsync(loginEndpoint, message);
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
}

*/