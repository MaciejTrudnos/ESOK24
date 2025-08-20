using System.Text.Json.Serialization;

namespace esok.api.DTO
{
    public class PostDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
