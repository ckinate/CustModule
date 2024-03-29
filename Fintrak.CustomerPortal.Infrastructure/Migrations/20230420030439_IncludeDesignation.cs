using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fintrak.CustomerPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IncludeDesignation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "CustomerContactPersons",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Designation",
                table: "CustomerContactPersons");
        }
    }
}
