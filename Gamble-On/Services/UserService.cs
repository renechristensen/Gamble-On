using System;
using System.Diagnostics;
using System.Net;
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
        private string RegisterEndpoint = "/User/CreateUser";
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

            if (response.IsSuccessStatusCode) // 200
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
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
                    // 423 home made
                    (HttpStatusCode)423 => new Exception("Your account is inactive. Please contact support."),
                    //... other cases
                    _ => new Exception("Unhandled error: " + response.StatusCode),
                };
            }
        }

        public async Task<bool> RegisterUserAsync(User userToRegister)
        {
            var jsonPayload = JsonConvert.SerializeObject(userToRegister);
            StringContent message = new(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(RegisterEndpoint, message);
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {

            var endpoint = $"/User/{userId}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<User>(jsonResponse);
                    return user;
                }
                else
                {
                    throw response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.BadRequest => new Exception("Bad Request"),
                        System.Net.HttpStatusCode.Unauthorized => new Exception("You are not authorized"),
                        System.Net.HttpStatusCode.NotFound => new Exception("User not found"),
                        _ => new Exception("Unhandled error: " + response.StatusCode),
                    };
                }
            }
            catch (HttpRequestException e)
            {
                throw new Exception("Network error occurred", e);
            }
            catch (TaskCanceledException e)
            {
                if (e.CancellationToken.IsCancellationRequested)
                    throw new Exception("Request was cancelled", e);
                else
                    throw new Exception("Request timed out", e);
            }
        }
    }
}