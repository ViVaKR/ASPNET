using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kimbumjun.Migrations
{
    public partial class AddTypeOfProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeOfProgram",
                table: "Mssql",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeOfProgram",
                table: "CSharps",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeOfProgram",
                table: "Angular",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeOfProgram",
                table: "Mssql");

            migrationBuilder.DropColumn(
                name: "TypeOfProgram",
                table: "CSharps");

            migrationBuilder.DropColumn(
                name: "TypeOfProgram",
                table: "Angular");
        }
    }
}
