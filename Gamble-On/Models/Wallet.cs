using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class Wallet
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("amount")]
        public float amount { get; set; }

        [JsonProperty("active")]
        public bool active { get; set; }

        [JsonProperty("userId")]
        public int userId { get; set; }
    }
}
