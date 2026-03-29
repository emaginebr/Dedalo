using System.Text.Json.Serialization;

namespace Dedalo.DTO.Menu
{
    public class MenuUpdateInfo
    {
        [JsonPropertyName("menuId")]
        public long MenuId { get; set; }
        [JsonPropertyName("parentId")]
        public long? ParentId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("linkType")]
        public LinkTypeEnum LinkType { get; set; }
        [JsonPropertyName("externalLink")]
        public string ExternalLink { get; set; }
        [JsonPropertyName("pageId")]
        public long? PageId { get; set; }
    }
}
