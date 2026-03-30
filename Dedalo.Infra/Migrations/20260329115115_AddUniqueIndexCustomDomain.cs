using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dedalo.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexCustomDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_dedalo_websites_custom_domain",
                table: "dedalo_websites",
                column: "custom_domain",
                unique: true,
                filter: "custom_domain IS NOT NULL AND custom_domain <> ''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_dedalo_websites_custom_domain",
                table: "dedalo_websites");
        }
    }
}
