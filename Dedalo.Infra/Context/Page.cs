using System;
using System.Collections.Generic;

namespace Dedalo.Infra.Context;

public partial class Page
{
    public long PageId { get; set; }

    public long WebsiteId { get; set; }

    public string PageSlug { get; set; }

    public string TemplatePageSlug { get; set; }

    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Website Website { get; set; }

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<Content> Contents { get; set; } = new List<Content>();
}
