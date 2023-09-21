using System;
using Newtonsoft.Json;
using System;

namespace Gamble_On.Models
{
    public class UserType
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("gameType")]
        public string gameType { get; set; }
    }
}

