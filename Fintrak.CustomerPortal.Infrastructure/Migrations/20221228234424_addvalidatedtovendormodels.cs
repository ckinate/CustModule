using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fintrak.CustomerPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addvalidatedtocustomermodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerSubCategories");

            migrationBuilder.AddColumn<bool>(
                name: "RequireSync",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SubCategoryIds",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubCategoryNames",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Validated",
                table: "CustomerAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequireSync",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SubCategoryIds",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SubCategoryNames",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Validated",
                table: "CustomerAccounts");

            migrationBuilder.CreateTable(
                name: "CustomerSubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubCategoryId = table.Column<int>(type: "int", nullable: false),
                    SubCategoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSubCategories_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSubCategories_CustomerId_SubCategoryId",
                table: "CustomerSubCategories",
                columns: new[] { "CustomerId", "SubCategoryId" });
        }
    }
}
