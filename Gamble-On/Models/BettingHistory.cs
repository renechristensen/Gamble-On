using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Text.Json.Serialization;

namespace Gamble_On.Models
{
    public class BettingHistory
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("walletID")]
        public int walletID { get; set; }
        [JsonPropertyName("bettingAmount")]
        public float bettingAmount { get; set; }
        [JsonPropertyName("playedGameId")]
        public int playedGameId { get; set; }
        [JsonPropertyName("createdTime")]
        public DateTime createdTime { get; set; }
        [JsonPropertyName("outcome")]
        public bool outcome { get; set; }
        [JsonPropertyName("bettingResult")]
        public float bettingResult { get; set; }
    }
}

