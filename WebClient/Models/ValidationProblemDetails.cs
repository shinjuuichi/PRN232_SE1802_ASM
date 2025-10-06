using System.Text.Json.Serialization;

namespace WebClient.Models
{
    public class ValidationProblemDetails
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("status")]
        public int? Status { get; set; }

        [JsonPropertyName("errors")]
        public Dictionary<string, string[]>? Errors { get; set; }

        [JsonPropertyName("traceId")]
        public string? TraceId { get; set; }
    }
}
