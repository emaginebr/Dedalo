using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dedalo.Infra.Context;

public partial class DedaloContext : DbContext
{
    public DedaloContext()
    {
    }

    public DedaloContext(DbContextOptions<DedaloContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Website> Websites { get; set; }

    public virtual DbSet<Page> Pages { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Content> Contents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Website>(entity =>
        {
            entity.HasKey(e => e.WebsiteId).HasName("dedalo_websites_pkey");

            entity.ToTable("dedalo_websites");

            entity.Property(e => e.WebsiteId).HasColumnName("website_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.WebsiteSlug)
                .IsRequired()
                .HasMaxLength(240)
                .HasColumnName("website_slug");
            entity.HasIndex(e => e.WebsiteSlug)
                .IsUnique()
                .HasDatabaseName("ix_dedalo_websites_slug");
            entity.Property(e => e.TemplateSlug)
                .HasMaxLength(240)
                .HasColumnName("template_slug");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(240)
                .HasColumnName("name");
            entity.Property(e => e.DomainType)
                .HasDefaultValue(1)
                .HasColumnName("domain_type");
            entity.Property(e => e.CustomDomain)
                .HasMaxLength(240)
                .HasColumnName("custom_domain");
            entity.Property(e => e.LogoUrl)
                .HasMaxLength(500)
                .HasColumnName("logo_url");
            entity.Property(e => e.Status)
                .HasDefaultValue(1)
                .HasColumnName("status");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasIndex(e => e.CustomDomain)
                .IsUnique()
                .HasDatabaseName("ix_dedalo_websites_custom_domain")
                .HasFilter("custom_domain IS NOT NULL AND custom_domain <> ''");

            entity.HasIndex(e => e.UserId)
                .HasDatabaseName("ix_dedalo_websites_user_id");
        });

        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(e => e.PageId).HasName("dedalo_pages_pkey");

            entity.ToTable("dedalo_pages");

            entity.Property(e => e.PageId).HasColumnName("page_id");
            entity.Property(e => e.WebsiteId).HasColumnName("website_id");
            entity.Property(e => e.PageSlug)
                .IsRequired()
                .HasMaxLength(240)
                .HasColumnName("page_slug");
            entity.Property(e => e.TemplatePageSlug)
                .HasMaxLength(240)
                .HasColumnName("template_page_slug");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(240)
                .HasColumnName("name");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Website).WithMany(p => p.Pages)
                .HasForeignKey(d => d.WebsiteId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_dedalo_page_website");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("dedalo_menus_pkey");

            entity.ToTable("dedalo_menus");

            entity.Property(e => e.MenuId).HasColumnName("menu_id");
            entity.Property(e => e.WebsiteId).HasColumnName("website_id");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(240)
                .HasColumnName("name");
            entity.Property(e => e.LinkType)
                .HasDefaultValue(1)
                .HasColumnName("link_type");
            entity.Property(e => e.ExternalLink)
                .HasMaxLength(500)
                .HasColumnName("external_link");
            entity.Property(e => e.PageId).HasColumnName("page_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Website).WithMany(p => p.Menus)
                .HasForeignKey(d => d.WebsiteId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_dedalo_menu_website");

            entity.HasOne(d => d.Page).WithMany(p => p.Menus)
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dedalo_menu_page");

            entity.HasOne(d => d.Parent).WithMany(p => p.Children)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dedalo_menu_parent");
        });

        modelBuilder.Entity<Content>(entity =>
        {
            entity.HasKey(e => e.ContentId).HasName("dedalo_contents_pkey");

            entity.ToTable("dedalo_contents");

            entity.Property(e => e.ContentId).HasColumnName("content_id");
            entity.Property(e => e.WebsiteId).HasColumnName("website_id");
            entity.Property(e => e.PageId).HasColumnName("page_id");
            entity.Property(e => e.ContentType)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("content_type");
            entity.Property(e => e.Index)
                .HasDefaultValue(0)
                .HasColumnName("index");
            entity.Property(e => e.ContentSlug)
                .HasMaxLength(240)
                .HasColumnName("content_slug");
            entity.Property(e => e.ContentValue)
                .HasColumnName("content_value");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Website).WithMany(p => p.Contents)
                .HasForeignKey(d => d.WebsiteId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_dedalo_content_website");

            entity.HasOne(d => d.Page).WithMany(p => p.Contents)
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_dedalo_content_page");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
