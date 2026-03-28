using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatchVault.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "ExternalIds",
                newName: "ExternalIdValue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExternalIdValue",
                table: "ExternalIds",
                newName: "Value");
        }
    }
}
