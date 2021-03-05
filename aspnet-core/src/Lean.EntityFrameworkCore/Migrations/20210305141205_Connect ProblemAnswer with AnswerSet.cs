using Microsoft.EntityFrameworkCore.Migrations;

namespace Lean.Migrations
{
    public partial class ConnectProblemAnswerwithAnswerSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProblemAnswerResults_UserLessonAnswerSets_UserLessonAnswerSetId",
                table: "UserProblemAnswerResults");

            migrationBuilder.AlterColumn<int>(
                name: "UserLessonAnswerSetId",
                table: "UserProblemAnswerResults",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProblemAnswerResults_UserLessonAnswerSets_UserLessonAnswerSetId",
                table: "UserProblemAnswerResults",
                column: "UserLessonAnswerSetId",
                principalTable: "UserLessonAnswerSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProblemAnswerResults_UserLessonAnswerSets_UserLessonAnswerSetId",
                table: "UserProblemAnswerResults");

            migrationBuilder.AlterColumn<int>(
                name: "UserLessonAnswerSetId",
                table: "UserProblemAnswerResults",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProblemAnswerResults_UserLessonAnswerSets_UserLessonAnswerSetId",
                table: "UserProblemAnswerResults",
                column: "UserLessonAnswerSetId",
                principalTable: "UserLessonAnswerSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
