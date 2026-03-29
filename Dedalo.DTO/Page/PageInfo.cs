using System;
using System.Text.Json.Serialization;

namespace Dedalo.DTO.Page
{
    public class PageInfo
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
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
