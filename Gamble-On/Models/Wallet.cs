using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class Wallet
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("amount")]
        public float amount { get; set; }
        [JsonPropertyName("active")]
        public bool active { get; set; }
        [JsonPropertyName("userId")]
        public int userId { get; set; }
    }
}