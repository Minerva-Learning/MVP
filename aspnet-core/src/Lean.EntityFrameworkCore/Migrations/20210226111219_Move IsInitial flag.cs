using Microsoft.EntityFrameworkCore.Migrations;

namespace Lean.Migrations
{
    public partial class MoveIsInitialflag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInitial",
                table: "Modules");

            migrationBuilder.AddColumn<bool>(
                name: "IsInitial",
                table: "Lessons",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInitial",
                table: "Lessons");

            migrationBuilder.AddColumn<bool>(
                name: "IsInitial",
                table: "Modules",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
