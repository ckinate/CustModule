using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fintrak.CustomerPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addtinmodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaxIdentificationNumber",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxIdentificationNumber",
                table: "Customers");
        }
    }
}
