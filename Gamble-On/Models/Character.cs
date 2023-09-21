using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class Character
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("gameId")]
        public int gameId { get; set; }

        [JsonProperty("odds")]
        public float odds { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }
    }
}