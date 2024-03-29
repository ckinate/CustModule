using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fintrak.CustomerPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerContactPersons_Customers_CustomerId",
                table: "CustomerContactPersons");

            migrationBuilder.CreateTable(
                name: "CustomerProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CustomerContactPersonId = table.Column<int>(type: "int", nullable: false),
                    OperationMode = table.Column<int>(type: "int", nullable: false),
                    CustomerAccountId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProducts_CustomerAccounts_CustomerAccountId",
                        column: x => x.CustomerAccountId,
                        principalTable: "CustomerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerProducts_CustomerContactPersons_CustomerContactPersonId",
                        column: x => x.CustomerContactPersonId,
                        principalTable: "CustomerContactPersons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerProducts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerProductCustomFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerProductId = table.Column<int>(type: "int", nullable: false),
                    CustomFieldId = table.Column<int>(type: "int", nullable: false),
                    CustomField = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsCompulsory = table.Column<bool>(type: "bit", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProductCustomFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProductCustomFields_CustomerProducts_CustomerProductId",
                        column: x => x.CustomerProductId,
                        principalTable: "CustomerProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerProductDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerProductId = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: true),
                    DocumentTypeName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasExpiryDate = table.Column<bool>(type: "bit", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProductDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProductDocuments_CustomerProducts_CustomerProductId",
                        column: x => x.CustomerProductId,
                        principalTable: "CustomerProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductCustomFields_CustomerProductId_CustomFieldId",
                table: "CustomerProductCustomFields",
                columns: new[] { "CustomerProductId", "CustomFieldId" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductDocuments_CustomerProductId_DocumentTypeName",
                table: "CustomerProductDocuments",
                columns: new[] { "CustomerProductId", "DocumentTypeName" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_CustomerAccountId",
                table: "CustomerProducts",
                column: "CustomerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_CustomerContactPersonId",
                table: "CustomerProducts",
                column: "CustomerContactPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_CustomerId_ProductId",
                table: "CustomerProducts",
                columns: new[] { "CustomerId", "ProductId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerContactPersons_Customers_CustomerId",
                table: "CustomerContactPersons",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerContactPersons_Customers_CustomerId",
                table: "CustomerContactPersons");

            migrationBuilder.DropTable(
                name: "CustomerProductCustomFields");

            migrationBuilder.DropTable(
                name: "CustomerProductDocuments");

            migrationBuilder.DropTable(
                name: "CustomerProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerContactPersons_Customers_CustomerId",
                table: "CustomerContactPersons",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
