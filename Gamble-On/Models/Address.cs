using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Gamble_On.Models
{
    public class Address
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("postalCode")]
        public int postalCode { get; set; }

        [JsonProperty("address1")]
        public string address1 { get; set; }
    }
}