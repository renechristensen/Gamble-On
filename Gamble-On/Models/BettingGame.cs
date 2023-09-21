using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class BettingGame
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("gameID")]
        public int gameID { get; set; }

        [JsonProperty("plannedTime")]
        public DateTime plannedTime { get; set; }
        [JsonProperty("winnerId")]
        public int winnerId { get; set; }
        [JsonProperty("beingPlayed")]
        public bool beingPlayed { get; set; }
    }
}