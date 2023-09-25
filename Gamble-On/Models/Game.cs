using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamble_On.Models
{
    public class Game
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("desc")]
        public string desc { get; set; }

        [JsonProperty("gameImage")]
        public string gameImage { get; set; }

        [JsonProperty("gameTypeId")]
        public int gameTypeId { get; set; }

        [JsonIgnore]
        public bool IsExpanded { get; set; } = false;

        [JsonProperty("characters")]
        public List<Character> characters { get; set; } = new List<Character>();
    }
}