using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;

namespace Gamble_On.Models
{
    public class Transaction
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("walletId")]
        public int walletId { get; set; }
        [JsonProperty("amount")]
        public float amount { get; set; }
        [JsonProperty("actionTime")]
        public DateTime actionTime { get; set; }
        [JsonIgnore]
        public string description { get; set; }
        [JsonIgnore]
        public Color backgroundColor { get; set; }
    }
}