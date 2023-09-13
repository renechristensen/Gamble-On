using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class Transaction
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("walletId")]
        public int walletId { get; set; }
        [JsonPropertyName("amount")]
        public int amount { get; set; }
        [JsonPropertyName("actionTime")]
        public DateTime actionTime { get; set; }
    }
}

