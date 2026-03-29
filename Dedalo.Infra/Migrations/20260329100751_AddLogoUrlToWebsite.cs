using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dedalo.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddLogoUrlToWebsite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "logo_url",
                table: "dedalo_websites",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "logo_url",
                table: "dedalo_websites");
        }
    }
}
