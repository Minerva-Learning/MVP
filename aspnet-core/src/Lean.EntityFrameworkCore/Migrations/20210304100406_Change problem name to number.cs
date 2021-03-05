using Microsoft.EntityFrameworkCore.Migrations;

namespace Lean.Migrations
{
    public partial class Changeproblemnametonumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from dbo.UserLearningProgresses");
            migrationBuilder.Sql("delete from Problems");
            migrationBuilder.DropIndex(
                name: "IX_Problems_LessonId",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Problems");

            migrationBuilder.RenameColumn(
                name: "LessonVideoUrl",
                table: "Lessons",
                newName: "LessonVideoHtml");

            migrationBuilder.RenameColumn(
                name: "ActivityVideoUrl",
                table: "Lessons",
                newName: "ActivityVideoHtml");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Problems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Problems_LessonId_Number",
                table: "Problems",
                columns: new[] { "LessonId", "Number" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Problems_LessonId_Number",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Problems");

            migrationBuilder.RenameColumn(
                name: "LessonVideoHtml",
                table: "Lessons",
                newName: "LessonVideoUrl");

            migrationBuilder.RenameColumn(
                name: "ActivityVideoHtml",
                table: "Lessons",
                newName: "ActivityVideoUrl");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Problems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Problems_LessonId",
                table: "Problems",
                column: "LessonId");
        }
    }
}
