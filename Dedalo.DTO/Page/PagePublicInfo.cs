using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Dedalo.DTO.Content;

namespace Dedalo.DTO.Page
{
    public class PagePublicInfo
    {
        [JsonPropertyName("pageId")]
        public long PageId { get; set; }
        [JsonPropertyName("websiteId")]
        public long WebsiteId { get; set; }
        [JsonPropertyName("pageSlug")]
        public string PageSlug { get; set; }
        [JsonPropertyName("templatePageSlug")]
        public string TemplatePageSlug { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("contents")]
        public Dictionary<string, List<ContentInfo>> Contents { get; set; } = new Dictionary<string, List<ContentInfo>>();
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
