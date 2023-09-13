using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class Game
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("desc")]
        public string desc { get; set; }
        [JsonPropertyName("gameTypeId")]
        public int gameTypeId { get; set; }

    }
}
