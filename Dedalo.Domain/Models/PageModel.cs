using System;
using System.Collections.Generic;

namespace Dedalo.Domain.Models
{
    public class PageModel
    {
        public long PageId { get; set; }
        public long WebsiteId { get; set; }
        public string PageSlug { get; set; }
        public string TemplatePageSlug { get; set; }
        public string Name { get; set; }
        public List<ContentModel> Contents { get; set; } = new List<ContentModel>();
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
