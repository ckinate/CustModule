using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fintrak.CustomerPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubsidiary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "CustomerSignatories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "CustomerSignatories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "CustomerSignatories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasSubsidiary",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrespondentBank",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubsidiary",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerCode",
                table: "CustomerProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerMis",
                table: "CustomerProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "CustomerProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "CustomerContactPersons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "CustomerContactPersons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "CustomerContactPersons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ParentId",
                table: "Customers",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Customers_ParentId",
                table: "Customers",
                column: "ParentId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Customers_ParentId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_ParentId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "CustomerSignatories");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "CustomerSignatories");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "CustomerSignatories");

            migrationBuilder.DropColumn(
                name: "HasSubsidiary",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsCorrespondentBank",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsSubsidiary",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerCode",
                table: "CustomerProducts");

            migrationBuilder.DropColumn(
                name: "CustomerMis",
                table: "CustomerProducts");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "CustomerProducts");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "CustomerContactPersons");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "CustomerContactPersons");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "CustomerContactPersons");
        }
    }
}
