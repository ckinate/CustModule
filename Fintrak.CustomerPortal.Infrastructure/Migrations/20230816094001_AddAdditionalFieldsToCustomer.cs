using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fintrak.CustomerPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdditionalFieldsToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerQuestionnaires");

            migrationBuilder.DropColumn(
                name: "IncludeLocalAccount",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UseForeignAccount",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "SubCategoryNames",
                table: "Customers",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "SubCategoryIds",
                table: "Customers",
                newName: "RegisterAddress2");

            migrationBuilder.RenameColumn(
                name: "RegisterAddress",
                table: "Customers",
                newName: "RegisterAddress1");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "Customers",
                newName: "SectorName");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Customers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Customers",
                newName: "StaffSize");

            migrationBuilder.AddColumn<string>(
                name: "BusinessNature",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChildInstitutionTypeId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChildInstitutionTypeName",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomCode",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IndustryId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IndustryName",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstitutionCode",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstitutionTypeId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstitutionTypeName",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoLocation",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SectorId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "CustomerDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CustomerHaveSigned",
                table: "CustomerDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DocumentTypeId",
                table: "CustomerDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "CustomerDocuments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasExpiryDate",
                table: "CustomerDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IssueDate",
                table: "CustomerDocuments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "CustomerDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequireCustomerSignature",
                table: "CustomerDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "CustomerDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ViewableByCustomer",
                table: "CustomerDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AccountType",
                table: "CustomerAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "CustomerAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerSignatories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumberCallCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSignatories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSignatories_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSignatories_CustomerId_Name",
                table: "CustomerSignatories",
                columns: new[] { "CustomerId", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerSignatories");

            migrationBuilder.DropColumn(
                name: "BusinessNature",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ChildInstitutionTypeId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ChildInstitutionTypeName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomCode",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IndustryId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IndustryName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "InstitutionCode",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "InstitutionTypeId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "InstitutionTypeName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LogoLocation",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SectorId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Completed",
                table: "CustomerDocuments");

            migrationBuilder.DropColumn(
                name: "CustomerHaveSigned",
                table: "CustomerDocuments");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "CustomerDocuments");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "CustomerDocuments");

            migrationBuilder.DropColumn(
                name: "HasExpiryDate",
                table: "CustomerDocuments");

            migrationBuilder.DropColumn(
                name: "IssueDate",
                table: "CustomerDocuments");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "CustomerDocuments");

            migrationBuilder.DropColumn(
                name: "RequireCustomerSignature",
                table: "CustomerDocuments");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "CustomerDocuments");

            migrationBuilder.DropColumn(
                name: "ViewableByCustomer",
                table: "CustomerDocuments");

            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "CustomerAccounts");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "CustomerAccounts");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Customers",
                newName: "SubCategoryNames");

            migrationBuilder.RenameColumn(
                name: "StaffSize",
                table: "Customers",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "SectorName",
                table: "Customers",
                newName: "CompanyName");

            migrationBuilder.RenameColumn(
                name: "RegisterAddress2",
                table: "Customers",
                newName: "SubCategoryIds");

            migrationBuilder.RenameColumn(
                name: "RegisterAddress1",
                table: "Customers",
                newName: "RegisterAddress");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Customers",
                newName: "CategoryName");

            migrationBuilder.AddColumn<bool>(
                name: "IncludeLocalAccount",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseForeignAccount",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CustomerQuestionnaires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCompulsory = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerQuestionnaires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerQuestionnaires_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerQuestionnaires_CustomerId_QuestionId",
                table: "CustomerQuestionnaires",
                columns: new[] { "CustomerId", "QuestionId" });
        }
    }
}
