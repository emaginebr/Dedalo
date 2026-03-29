using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dedalo.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dedalo_websites",
                columns: table => new
                {
                    website_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    website_slug = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    template_slug = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    name = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    domain_type = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    custom_domain = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("dedalo_websites_pkey", x => x.website_id);
                });

            migrationBuilder.CreateTable(
                name: "dedalo_pages",
                columns: table => new
                {
                    page_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    website_id = table.Column<long>(type: "bigint", nullable: false),
                    page_slug = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    template_page_slug = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    name = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("dedalo_pages_pkey", x => x.page_id);
                    table.ForeignKey(
                        name: "fk_dedalo_page_website",
                        column: x => x.website_id,
                        principalTable: "dedalo_websites",
                        principalColumn: "website_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dedalo_contents",
                columns: table => new
                {
                    content_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    website_id = table.Column<long>(type: "bigint", nullable: false),
                    page_id = table.Column<long>(type: "bigint", nullable: false),
                    content_type = table.Column<int>(type: "integer", nullable: false),
                    index = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    content_slug = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    content_value = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("dedalo_contents_pkey", x => x.content_id);
                    table.ForeignKey(
                        name: "fk_dedalo_content_page",
                        column: x => x.page_id,
                        principalTable: "dedalo_pages",
                        principalColumn: "page_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_dedalo_content_website",
                        column: x => x.website_id,
                        principalTable: "dedalo_websites",
                        principalColumn: "website_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dedalo_menus",
                columns: table => new
                {
                    menu_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    website_id = table.Column<long>(type: "bigint", nullable: false),
                    parent_id = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    link_type = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    external_link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    page_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("dedalo_menus_pkey", x => x.menu_id);
                    table.ForeignKey(
                        name: "fk_dedalo_menu_page",
                        column: x => x.page_id,
                        principalTable: "dedalo_pages",
                        principalColumn: "page_id");
                    table.ForeignKey(
                        name: "fk_dedalo_menu_parent",
                        column: x => x.parent_id,
                        principalTable: "dedalo_menus",
                        principalColumn: "menu_id");
                    table.ForeignKey(
                        name: "fk_dedalo_menu_website",
                        column: x => x.website_id,
                        principalTable: "dedalo_websites",
                        principalColumn: "website_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dedalo_contents_page_id",
                table: "dedalo_contents",
                column: "page_id");

            migrationBuilder.CreateIndex(
                name: "IX_dedalo_contents_website_id",
                table: "dedalo_contents",
                column: "website_id");

            migrationBuilder.CreateIndex(
                name: "IX_dedalo_menus_page_id",
                table: "dedalo_menus",
                column: "page_id");

            migrationBuilder.CreateIndex(
                name: "IX_dedalo_menus_parent_id",
                table: "dedalo_menus",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_dedalo_menus_website_id",
                table: "dedalo_menus",
                column: "website_id");

            migrationBuilder.CreateIndex(
                name: "IX_dedalo_pages_website_id",
                table: "dedalo_pages",
                column: "website_id");

            migrationBuilder.CreateIndex(
                name: "ix_dedalo_websites_slug",
                table: "dedalo_websites",
                column: "website_slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_dedalo_websites_user_id",
                table: "dedalo_websites",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dedalo_contents");

            migrationBuilder.DropTable(
                name: "dedalo_menus");

            migrationBuilder.DropTable(
                name: "dedalo_pages");

            migrationBuilder.DropTable(
                name: "dedalo_websites");
        }
    }
}
