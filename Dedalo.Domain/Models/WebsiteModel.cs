using Dedalo.DTO.Website;
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

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
        public string Css { get; set; }
        public WebsiteStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void GenerateSlug(string name)
        {
            WebsiteSlug = ToSlug(name);
        }

        public void EnsureSlug()
        {
            if (string.IsNullOrWhiteSpace(WebsiteSlug))
                GenerateSlug(Name);
            else
                WebsiteSlug = ToSlug(WebsiteSlug);
        }

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

        private static string ToSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            var slug = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"[\s-]+", "-");
            slug = slug.Trim('-');
            return slug;
        }
    }
}
