using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kimbumjun.Migrations
{
    public partial class RemoveSubtitleProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTitle",
                table: "Mssql");

            migrationBuilder.DropColumn(
                name: "SubTitle",
                table: "CSharps");

            migrationBuilder.DropColumn(
                name: "SubTitle",
                table: "Angular");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubTitle",
                table: "Mssql",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubTitle",
                table: "CSharps",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubTitle",
                table: "Angular",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
