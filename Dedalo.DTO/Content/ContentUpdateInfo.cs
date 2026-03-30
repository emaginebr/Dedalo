using System.Text.Json.Serialization;

namespace Dedalo.DTO.Content
{
    public class ContentUpdateInfo
    {
        [JsonPropertyName("contentId")]
        public long ContentId { get; set; }
        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("contentSlug")]
        public string ContentSlug { get; set; }
        [JsonPropertyName("contentValue")]
        public string ContentValue { get; set; }
    }
}
