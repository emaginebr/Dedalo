using System;

namespace Dedalo.Infra.Context;

public partial class Content
{
    public long ContentId { get; set; }

    public long WebsiteId { get; set; }

    public long PageId { get; set; }

    public int ContentType { get; set; }

    public int Index { get; set; }

    public string ContentSlug { get; set; }

    public string ContentValue { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Website Website { get; set; }

    public virtual Page Page { get; set; }
}
