using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dedalo.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddCssToWebsite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "css",
                table: "dedalo_websites",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "css",
                table: "dedalo_websites");
        }
    }
}
