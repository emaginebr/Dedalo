using System;
using System.Collections.Generic;

namespace Dedalo.Infra.Context;

public partial class Website
{
    public long WebsiteId { get; set; }

    public long UserId { get; set; }

    public string WebsiteSlug { get; set; }

    public string TemplateSlug { get; set; }

    public string Name { get; set; }

    public int DomainType { get; set; }

    public string CustomDomain { get; set; }

    public string LogoUrl { get; set; }

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Page> Pages { get; set; } = new List<Page>();

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<Content> Contents { get; set; } = new List<Content>();
}
