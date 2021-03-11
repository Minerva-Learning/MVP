using Microsoft.EntityFrameworkCore.Migrations;

namespace Lean.Migrations
{
    public partial class AddFlowRulepriority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "FlowRules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "FlowRules");
        }
    }
}
