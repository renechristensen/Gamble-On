using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class BettingHistoryAlter
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

        [JsonProperty("bettingCharacter")]
        public string bettingCharacter { get; set; }
        [JsonProperty("bettingGame")]
        public BettingGameAlter bettingGame { get; set; }

        public class BettingGameAlter
        {
            [JsonProperty("plannedTime")]

            public DateTime plannedTime { get; set; }
            [JsonProperty("beingPlayed")]
            public bool? beingPlayed { get; set; }
            [JsonProperty("game")]
            public GameAlter game { get; set; }
        }
        public class GameAlter
        {
            [JsonProperty("name")]
            public string name { get; set; }
        }
    }
}
