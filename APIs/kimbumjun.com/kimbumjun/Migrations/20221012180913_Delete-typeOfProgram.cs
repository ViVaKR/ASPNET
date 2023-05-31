using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kimbumjun.Migrations
{
    public partial class DeletetypeOfProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeOfProgram",
                table: "ViVs");

            migrationBuilder.DropColumn(
                name: "TypeOfProgram",
                table: "CSharps");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeOfProgram",
                table: "ViVs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeOfProgram",
                table: "CSharps",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
