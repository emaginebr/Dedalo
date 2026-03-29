using Dedalo.DTO.Website;
using System;

namespace Dedalo.Domain.Models
{
    public class WebsiteModel
    {
        public long WebsiteId { get; set; }
        public long UserId { get; set; }
        public string WebsiteSlug { get; set; }
        public string TemplateSlug { get; set; }
        public string Name { get; set; }
        public DomainTypeEnum DomainType { get; set; }
        public string CustomDomain { get; set; }
        public string LogoUrl { get; set; }
        public WebsiteStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void SetOwner(long userId)
        {
            UserId = userId;
        }

        public void ValidateOwnership(long userId)
        {
            if (UserId != userId)
                throw new UnauthorizedAccessException("Access denied: website does not belong to this user");
        }

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
