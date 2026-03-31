using System.Text.Json.Serialization;

namespace Dedalo.DTO.Website
{
    public class WebsiteInsertInfo
    {
        [JsonPropertyName("templateSlug")]
        public string TemplateSlug { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("domainType")]
        public DomainTypeEnum DomainType { get; set; }
        [JsonPropertyName("customDomain")]
        public string? CustomDomain { get; set; }
        [JsonPropertyName("css")]
        public string? Css { get; set; }
    }
}
