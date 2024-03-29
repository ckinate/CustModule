using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fintrak.CustomerPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSettlementBank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SettlementBankId",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "SettlementBankCode",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SettlementBankName",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SettlementBankCode",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SettlementBankName",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "SettlementBankId",
                table: "Customers",
                type: "int",
                nullable: true);
        }
    }
}
