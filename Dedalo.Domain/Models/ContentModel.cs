using System;

namespace Dedalo.Domain.Models
{
    public class ContentModel
    {
        public long ContentId { get; set; }
        public long WebsiteId { get; set; }
        public long PageId { get; set; }
        public string ContentType { get; set; }
        public int Index { get; set; }
        public string ContentSlug { get; set; }
        public string ContentValue { get; set; }
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
