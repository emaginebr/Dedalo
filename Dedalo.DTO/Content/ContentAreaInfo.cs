using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dedalo.DTO.Content
{
    public class ContentAreaInfo
    {
        [JsonPropertyName("websiteId")]
        public long WebsiteId { get; set; }
        [JsonPropertyName("pageId")]
        public long PageId { get; set; }
        [JsonPropertyName("contentSlug")]
        public string ContentSlug { get; set; }
        [JsonPropertyName("items")]
        public List<ContentAreaItemInfo> Items { get; set; } = new List<ContentAreaItemInfo>();
    }

    public class ContentAreaItemInfo
    {
        [JsonPropertyName("contentId")]
        public long ContentId { get; set; }
        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("contentValue")]
        public string ContentValue { get; set; }
    }
}
