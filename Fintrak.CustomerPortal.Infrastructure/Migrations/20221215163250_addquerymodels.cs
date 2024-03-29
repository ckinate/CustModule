using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fintrak.CustomerPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addquerymodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Queries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceType = table.Column<int>(type: "int", nullable: false),
                    ResourceReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    QueryMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    QueryInitiator = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QueryTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QueryResponse = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPending = table.Column<bool>(type: "bit", nullable: false),
                    ResponseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queries", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Queries");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Customers");
        }
    }
}
