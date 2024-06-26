using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bkpDN.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccountSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Accounts",
                newName: "Deleted_at");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Accounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_at",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Happened_at",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated_at",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created_at",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Happened_at",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Updated_at",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "Deleted_at",
                table: "Accounts",
                newName: "DeletedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
