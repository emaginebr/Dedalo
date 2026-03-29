using System;
using System.Collections.Generic;

namespace Dedalo.Infra.Context;

public partial class Menu
{
    public long MenuId { get; set; }

    public long WebsiteId { get; set; }

    public long? ParentId { get; set; }

    public string Name { get; set; }

    public int LinkType { get; set; }

    public string ExternalLink { get; set; }

    public long? PageId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Website Website { get; set; }

    public virtual Page Page { get; set; }

    public virtual Menu Parent { get; set; }

    public virtual ICollection<Menu> Children { get; set; } = new List<Menu>();
}
