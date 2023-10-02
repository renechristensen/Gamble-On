using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class User
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("dateOfBirth")]
        public DateTime dateOfBirth { get; set; }

        [JsonProperty("firstName")]
        public string firstName { get; set; }

        [JsonProperty("lastName")]
        public string lastName { get; set; }

        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("password")]
        public string password { get; set; }

        [JsonProperty("walletId")]
        public int walletId { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("phoneNumber")]
        public int phoneNumber { get; set; }

        [JsonProperty("active")]
        public bool active { get; set; }

        [JsonProperty("userTypeId")]
        public int userTypeId { get; set; }

        [JsonProperty("addressId")]
        public int addressId { get; set; }

        [JsonProperty("address")]
        public string address { get; set; }
        [JsonIgnore]
        public Address Address { get; set; }

        [JsonProperty("userType1")]
        public string UserType { get; set; }
    }
}
