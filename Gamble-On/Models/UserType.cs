using System.Text.Json.Serialization;

namespace Gamble_On.Models
{
    public class UserType
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("userType")]
        public string userType { get; set; }
    }
}
