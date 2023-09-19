using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace Gamble_On.Models
{
    public class Address
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("postalCode")]
        public int postalCode { get; set; }

        [JsonPropertyName("address1")]
        public string address1 { get; set; }
    }
}


