using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class Character
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("gameId")]
        public int gameId  { get; set; }
        [JsonPropertyName("odds")]
        public float odds { get; set; }
        [JsonPropertyName("name")]
        public string name { get; set; }
    }
}
