using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kimbumjun.Migrations
{
    public partial class AddCategoryNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "ViVs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "CSharps",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "ViVs");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "CSharps");
        }
    }
}
