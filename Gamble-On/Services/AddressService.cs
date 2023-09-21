using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Gamble_On.Models;
using Newtonsoft.Json;

namespace Gamble_On.Services
{
    public class AddressService : IAddressService
    {
        private readonly HttpClient _httpClient;
        private string AddressesEndpoint = "/address"; // Replace with your actual API endpoint

        public AddressService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Token is missing or invalid");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(AddressesEndpoint);
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

            if (response.IsSuccessStatusCode) // 200 OK
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Address>>(responseString);
            }
            else
            {
                throw response.StatusCode switch
                {
                    HttpStatusCode.BadRequest => new Exception("Bad Request"),
                    HttpStatusCode.Unauthorized => new Exception("You are not authorized"),
                    HttpStatusCode.NotFound => new Exception("Addresses not found"),
                    _ => new Exception("Unhandled error: " + response.StatusCode),
                };
            }
        }
    }
}



