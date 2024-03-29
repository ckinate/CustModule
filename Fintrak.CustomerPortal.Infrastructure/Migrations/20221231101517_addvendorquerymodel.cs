using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fintrak.CustomerPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addcustomerquerymodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QueryTo",
                table: "Queries");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Queries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Queries_CustomerId",
                table: "Queries",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Queries_Customers_CustomerId",
                table: "Queries",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Queries_Customers_CustomerId",
                table: "Queries");

            migrationBuilder.DropIndex(
                name: "IX_Queries_CustomerId",
                table: "Queries");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Queries");

            migrationBuilder.AddColumn<string>(
                name: "QueryTo",
                table: "Queries",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
