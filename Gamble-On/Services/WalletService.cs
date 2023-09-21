using System.Threading.Tasks;
using Gamble_On.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using Debug = System.Diagnostics.Debug;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;

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
        
        // Get Betting histories
        public async Task<List<BettingHistory>> GetBettingHistoryByUserIdAsync(int userId)
        {
            var endpoint = $"/BettingHistory/UserId/{userId}";
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    List<BettingHistory> bettingHistories = new();
                    try
                    {
                        bettingHistories = JsonConvert.DeserializeObject<List<BettingHistory>>(jsonResponse);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message); 
                    }
                    return bettingHistories;
                }
                else
                {
                    // Log or handle the error response here
                    return new List<BettingHistory>(); // Return an empty list or throw an exception
                }
            }
            catch (HttpRequestException e)
            {
                // Handle specific network exceptions here if needed
                throw new Exception("Network error occurred", e);
            }
        }

        // Get Transactions
        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId)
        {
            var endpoint = $"/Transaction/UserId/{userId}";
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("JSON Response: " + jsonResponse);
                    List<Transaction> transactions = new();
                    try
                    {
                        transactions = JsonConvert.DeserializeObject<List<Transaction>>(jsonResponse);
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine("Error during deserialization: " + e.Message);
                    }
                   
                    return transactions;
                }
                else
                {
                    throw new Exception("Error with Deserialize");
                }
            }
            catch (HttpRequestException e)
            {
                // Handle specific network exceptions here if needed
                throw new Exception("Network error occurred", e);
            }
        }

    }
}