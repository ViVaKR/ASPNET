using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kimbumjun.Migrations
{
    public partial class MssqlAngularCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "CSharps");

            migrationBuilder.DropColumn(
                name: "Writer",
                table: "CSharps");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfWriting",
                table: "CSharps",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Angular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SubTitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Contents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfWriting = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Angular", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mssql",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SubTitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Contents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfWriting = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mssql", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Angular");

            migrationBuilder.DropTable(
                name: "Mssql");

            migrationBuilder.DropColumn(
                name: "DateOfWriting",
                table: "CSharps");

            migrationBuilder.AddColumn<long>(
                name: "Count",
                table: "CSharps",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Writer",
                table: "CSharps",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
