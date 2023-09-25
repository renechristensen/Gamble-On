using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gamble_On.Models
{
    public class BettingGame
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("gameId")]
        public int GameId { get; set; }

        [JsonProperty("plannedTime")]
        public DateTime PlannedTime { get; set; }

        [JsonProperty("winnerId")]
        public int? WinnerId { get; set; } // Made it nullable to handle null values

        [JsonProperty("beingPlayed")]
        public bool BeingPlayed { get; set; }

        [JsonProperty("game")]
        public Game Game { get; set; }

     }
}