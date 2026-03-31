using System.Text.Json.Serialization;

namespace Dedalo.DTO.Page
{
    public class PageInsertInfo
    {
        [JsonPropertyName("websiteId")]
        public long WebsiteId { get; set; }
        [JsonPropertyName("templatePageSlug")]
        public string TemplatePageSlug { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
