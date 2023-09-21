using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gamble_On.Models;
using Newtonsoft.Json;

namespace Gamble_On.Services
{
    public class AddressService : BaseService, IAddressService
    {
        private readonly string AddressesEndpoint = "/address"; // Replace with your actual API endpoint

        public AddressService(HttpClient httpClient, IAuthorizationService authorizationService)
            : base(httpClient, authorizationService)
        {
        }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync()
        {
            HttpResponseMessage response = await ExecuteHttpRequestAsync(() => _httpClient.GetAsync(AddressesEndpoint));

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Address>>(responseString);
            }
            else
            {
                throw new Exception($"Failed to fetch addresses: {response.StatusCode}");
            }
        }
    }
}
