using System.Text.Json.Serialization;

namespace Dedalo.DTO.Page
{
    public class PageUpdateInfo
    {
        [JsonPropertyName("pageId")]
        public long PageId { get; set; }
        [JsonPropertyName("pageSlug")]
        public string PageSlug { get; set; }
        [JsonPropertyName("templatePageSlug")]
        public string TemplatePageSlug { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
