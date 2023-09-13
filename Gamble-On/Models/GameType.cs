﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gamble_On.Models
{
    public class GameType
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("gameType")]
        public string gameType { get; set; }
    }
}
