using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class Game
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("desc")]
        public string desc { get; set; }

        [JsonProperty("gameImage")]
        public string gameImage { get; set; }

        [JsonProperty("gameTypeId")]
        public int gameTypeId { get; set; }
    }
}
