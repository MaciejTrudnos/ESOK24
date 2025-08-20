using System.Text.Json.Serialization;

namespace esok.api.DTO
{
    public class ChatbotAnswerDTO
    {
        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("start")]
        public int Start { get; set; }

        [JsonPropertyName("end")]
        public int End { get; set; }

        [JsonPropertyName("answer")]
        public string Answer { get; set; }
    }
}
