using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fintrak.CustomerPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateinvitationmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EntryDate",
                table: "Invitations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UsedDate",
                table: "Invitations",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntryDate",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "UsedDate",
                table: "Invitations");
        }
    }
}
