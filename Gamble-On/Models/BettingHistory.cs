using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class BettingHistory
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("walletID")]
        public int walletID { get; set; }

        [JsonProperty("bettingAmount")]
        public float bettingAmount { get; set; }

        [JsonProperty("bettingGameId")]
        public int bettingGameId { get; set; }

        [JsonProperty("createdTime")]
        public DateTime createdTime { get; set; }

        [JsonProperty("outcome")]
        public bool? outcome { get; set; }

        [JsonProperty("bettingResult")]
        public float? bettingResult { get; set; }

        [JsonProperty("bettingCharacterId")]
        public int bettingCharacterId { get; set; }
        [JsonIgnore]
        public string gameResultSoFar { get; set; }
    }
}
