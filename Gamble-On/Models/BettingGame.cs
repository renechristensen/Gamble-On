using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class BettingGame
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("gameID")]
        public int gameID { get; set; }
        [JsonPropertyName("plannedTime")]
        public DateTime plannedTime { get; set; }
    }
}
