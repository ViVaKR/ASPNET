using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kimbumjun.Migrations
{
    public partial class Deletewritingdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfWriting",
                table: "Mssql");

            migrationBuilder.DropColumn(
                name: "DateOfWriting",
                table: "CSharps");

            migrationBuilder.DropColumn(
                name: "DateOfWriting",
                table: "Angular");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfWriting",
                table: "Mssql",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfWriting",
                table: "CSharps",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfWriting",
                table: "Angular",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
