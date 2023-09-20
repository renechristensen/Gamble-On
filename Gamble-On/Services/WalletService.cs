using System.Threading.Tasks;
using Gamble_On.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using Debug = System.Diagnostics.Debug;
using System.Net.Http;
using System.Text;
namespace Gamble_On.Services
{
    public class WalletService : IWalletService
    {
        private readonly HttpClient _httpClient;

        public WalletService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // load the wallet
        public async Task<Wallet> GetWalletByUserIdAsync(int userId)
        {
            var endpoint = $"/Wallet/UserId/{userId}";
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(content);
           
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Wallet>(jsonResponse);
                } catch (NullReferenceException ex)
                {
                    Debug.WriteLine(ex.Message);
                    return null;
                }

            }
            else
            {
                // Handle errors as appropriate
                return null;
            }
        }

        //deposit cash/tokens
        public async Task<bool> DepositAsync(int userId, float amount)
        {
            Wallet wallet = await GetWalletByUserIdAsync(userId);
            if (wallet != null)
            {
                wallet.amount += amount;

                var walletUpdateEndpoint = $"/Wallet/{wallet.id}";

                var jsonPayload = JsonConvert.SerializeObject(wallet);
                StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");
                Debug.WriteLine(content);
                HttpResponseMessage response;
                try
                {
                    // Attempt to send the PUT request to update the wallet
                    response = await _httpClient.PutAsync(walletUpdateEndpoint, content);

                    // Check if the request was not successful
                    if (!response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Error updating wallet. Response: {responseContent}");
                        return false;
                    }
                }
                catch (HttpRequestException e)
                {
                    // Handle specific network exceptions here if needed
                    Debug.WriteLine($"Network error occurred: {e.Message}");
                    return false;
                }

                return true;
            }
            return false;
        }

        // withdraw cash/tokens
        public async Task<bool> WithdrawAsync(int userId, float amount)
        {
            Wallet wallet = await GetWalletByUserIdAsync(userId);
            if (wallet != null)
            {
                wallet.amount -= amount;

                var walletUpdateEndpoint = $"/Wallet/{wallet.id}";

                var jsonPayload = JsonConvert.SerializeObject(wallet);
                StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");
                Debug.WriteLine(content);
                HttpResponseMessage response;
                try
                {
                    // Attempt to send the PUT request to update the wallet
                    response = await _httpClient.PutAsync(walletUpdateEndpoint, content);

                    // Check if the request was not successful
                    if (!response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Error updating wallet. Response: {responseContent}");
                        return false;
                    }
                }
                catch (HttpRequestException e)
                {
                    // Handle specific network exceptions here if needed
                    Debug.WriteLine($"Network error occurred: {e.Message}");
                    return false;
                }

                return true;
            }
            return false;
        }


    }
}

