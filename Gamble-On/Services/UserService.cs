using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Gamble_On.Models;
using Newtonsoft.Json;

namespace Gamble_On.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<String> LoginAsync(string email, string password)
        {
            var loginEndpoint = "/User/UserLogin"; // No need to prefix with BaseUrl, it's set during HttpClient configuration

            var jsonPayload = JsonConvert.SerializeObject(new
            {
                email = email,
                password = password
            });

            StringContent message = new(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            try
            {
                // Attempt to send the POST request
                response = await _httpClient.PostAsync(loginEndpoint, message);
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(content);
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

            if (response.IsSuccessStatusCode) // 2000
            {
                var token = await response.Content.ReadAsStringAsync();
                return token;
                //var user = JsonConvert.DeserializeObject<User>(jsonResponse);
                //return user;
            }
            else {
                throw response.StatusCode switch
                {
                    // 400
                    System.Net.HttpStatusCode.BadRequest => new Exception("Bad Request"),
                    // 401
                    System.Net.HttpStatusCode.Unauthorized => new Exception("You are not authorized"),
                    //... other cases
                    _ => new Exception("Unhandled error: " + response.StatusCode),
                };
            }
        }
    }
}