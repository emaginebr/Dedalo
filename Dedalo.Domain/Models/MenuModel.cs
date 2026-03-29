using Dedalo.DTO.Menu;
using System;

namespace Dedalo.Domain.Models
{
    public class MenuModel
    {
        public long MenuId { get; set; }
        public long WebsiteId { get; set; }
        public long? ParentId { get; set; }
        public string Name { get; set; }
        public LinkTypeEnum LinkType { get; set; }
        public string ExternalLink { get; set; }
        public long? PageId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void MarkCreated()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public void MarkUpdated()
        {
            UpdatedAt = DateTime.Now;
        }
    }
}
