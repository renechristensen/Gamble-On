using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("firstName")]
        public string firstName { get; set; }
        [JsonPropertyName("lastName")]
        public string lastName { get; set; }
        [JsonPropertyName("username")]
        public string username { get; set; } // added this guys :D
        [JsonPropertyName("password")]
        public string password { get; set; }
        [JsonPropertyName("walletId")]
        public int walletId { get; set; }
        [JsonPropertyName("email")]
        public string email { get; set; }
        [JsonPropertyName("phoneNumber")]
        public int phoneNumber { get; set; }
        [JsonPropertyName("active")]
        public bool active { get; set; }
        [JsonPropertyName("userTypeId")]
        public int userTypeId { get; set; }
        [JsonPropertyName("addressId")]
        public int addressId { get; set; }
        [JsonPropertyName("Token")]
        public string Token { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }

        [JsonPropertyName("userType")]
        public UserType UserType { get; set; }
    }
}
