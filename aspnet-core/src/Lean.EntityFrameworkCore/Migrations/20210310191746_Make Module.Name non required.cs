using Microsoft.EntityFrameworkCore.Migrations;

namespace Lean.Migrations
{
    public partial class MakeModuleNamenonrequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProblemAnswerOptions_ProblemId",
                table: "ProblemAnswerOptions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemAnswerOptions_ProblemId_IsCorrect",
                table: "ProblemAnswerOptions",
                columns: new[] { "ProblemId", "IsCorrect" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProblemAnswerOptions_ProblemId_IsCorrect",
                table: "ProblemAnswerOptions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProblemAnswerOptions_ProblemId",
                table: "ProblemAnswerOptions",
                column: "ProblemId");
        }
    }
}
