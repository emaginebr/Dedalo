using System;
using System.Text.Json.Serialization;

namespace Dedalo.DTO.Content
{
    public class ContentInfo
    {
        [JsonPropertyName("contentId")]
        public long ContentId { get; set; }
        [JsonPropertyName("websiteId")]
        public long WebsiteId { get; set; }
        [JsonPropertyName("pageId")]
        public long PageId { get; set; }
        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("contentSlug")]
        public string ContentSlug { get; set; }
        [JsonPropertyName("contentValue")]
        public string ContentValue { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
