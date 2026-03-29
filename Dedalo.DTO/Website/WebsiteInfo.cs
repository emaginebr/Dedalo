using System;
using System.Text.Json.Serialization;

namespace Dedalo.DTO.Website
{
    public class WebsiteInfo
    {
        [JsonPropertyName("websiteId")]
        public long WebsiteId { get; set; }
        [JsonPropertyName("userId")]
        public long UserId { get; set; }
        [JsonPropertyName("websiteSlug")]
        public string WebsiteSlug { get; set; }
        [JsonPropertyName("templateSlug")]
        public string TemplateSlug { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("domainType")]
        public DomainTypeEnum DomainType { get; set; }
        [JsonPropertyName("customDomain")]
        public string CustomDomain { get; set; }
        [JsonPropertyName("logoUrl")]
        public string LogoUrl { get; set; }
        [JsonPropertyName("status")]
        public WebsiteStatusEnum Status { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
