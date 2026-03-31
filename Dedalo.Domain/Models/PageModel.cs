using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

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

        public void GenerateSlug(string name)
        {
            PageSlug = ToSlug(name);
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
