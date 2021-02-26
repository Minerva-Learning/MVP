using Microsoft.EntityFrameworkCore.Migrations;

namespace Lean.Migrations
{
    public partial class Addmissingproblemansweroptionentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_Module_ModuleId",
                table: "Lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_Module_Course_CourseId",
                table: "Module");

            migrationBuilder.DropForeignKey(
                name: "FK_Problem_ProblemSet_ProblemSetId",
                table: "Problem");

            migrationBuilder.DropForeignKey(
                name: "FK_ProblemAnswerOption_Problem_ProblemId",
                table: "ProblemAnswerOption");

            migrationBuilder.DropForeignKey(
                name: "FK_ProblemSet_Lesson_LessonId",
                table: "ProblemSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProblemSet",
                table: "ProblemSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProblemAnswerOption",
                table: "ProblemAnswerOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Problem",
                table: "Problem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Module",
                table: "Module");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lesson",
                table: "Lesson");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.RenameTable(
                name: "ProblemSet",
                newName: "ProblemSets");

            migrationBuilder.RenameTable(
                name: "ProblemAnswerOption",
                newName: "ProblemAnswerOptions");

            migrationBuilder.RenameTable(
                name: "Problem",
                newName: "Problems");

            migrationBuilder.RenameTable(
                name: "Module",
                newName: "Modules");

            migrationBuilder.RenameTable(
                name: "Lesson",
                newName: "Lessons");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "Courses");

            migrationBuilder.RenameIndex(
                name: "IX_ProblemSet_LessonId",
                table: "ProblemSets",
                newName: "IX_ProblemSets_LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_ProblemAnswerOption_ProblemId",
                table: "ProblemAnswerOptions",
                newName: "IX_ProblemAnswerOptions_ProblemId");

            migrationBuilder.RenameIndex(
                name: "IX_Problem_ProblemSetId",
                table: "Problems",
                newName: "IX_Problems_ProblemSetId");

            migrationBuilder.RenameIndex(
                name: "IX_Module_CourseId",
                table: "Modules",
                newName: "IX_Modules_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_ModuleId",
                table: "Lessons",
                newName: "IX_Lessons_ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProblemSets",
                table: "ProblemSets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProblemAnswerOptions",
                table: "ProblemAnswerOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Problems",
                table: "Problems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Modules",
                table: "Modules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lessons",
                table: "Lessons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Modules_ModuleId",
                table: "Lessons",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Courses_CourseId",
                table: "Modules",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemAnswerOptions_Problems_ProblemId",
                table: "ProblemAnswerOptions",
                column: "ProblemId",
                principalTable: "Problems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_ProblemSets_ProblemSetId",
                table: "Problems",
                column: "ProblemSetId",
                principalTable: "ProblemSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemSets_Lessons_LessonId",
                table: "ProblemSets",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Modules_ModuleId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Courses_CourseId",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_ProblemAnswerOptions_Problems_ProblemId",
                table: "ProblemAnswerOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Problems_ProblemSets_ProblemSetId",
                table: "Problems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProblemSets_Lessons_LessonId",
                table: "ProblemSets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProblemSets",
                table: "ProblemSets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Problems",
                table: "Problems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProblemAnswerOptions",
                table: "ProblemAnswerOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Modules",
                table: "Modules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lessons",
                table: "Lessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.RenameTable(
                name: "ProblemSets",
                newName: "ProblemSet");

            migrationBuilder.RenameTable(
                name: "Problems",
                newName: "Problem");

            migrationBuilder.RenameTable(
                name: "ProblemAnswerOptions",
                newName: "ProblemAnswerOption");

            migrationBuilder.RenameTable(
                name: "Modules",
                newName: "Module");

            migrationBuilder.RenameTable(
                name: "Lessons",
                newName: "Lesson");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Course");

            migrationBuilder.RenameIndex(
                name: "IX_ProblemSets_LessonId",
                table: "ProblemSet",
                newName: "IX_ProblemSet_LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_Problems_ProblemSetId",
                table: "Problem",
                newName: "IX_Problem_ProblemSetId");

            migrationBuilder.RenameIndex(
                name: "IX_ProblemAnswerOptions_ProblemId",
                table: "ProblemAnswerOption",
                newName: "IX_ProblemAnswerOption_ProblemId");

            migrationBuilder.RenameIndex(
                name: "IX_Modules_CourseId",
                table: "Module",
                newName: "IX_Module_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_ModuleId",
                table: "Lesson",
                newName: "IX_Lesson_ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProblemSet",
                table: "ProblemSet",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Problem",
                table: "Problem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProblemAnswerOption",
                table: "ProblemAnswerOption",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Module",
                table: "Module",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lesson",
                table: "Lesson",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_Module_ModuleId",
                table: "Lesson",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Course_CourseId",
                table: "Module",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Problem_ProblemSet_ProblemSetId",
                table: "Problem",
                column: "ProblemSetId",
                principalTable: "ProblemSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemAnswerOption_Problem_ProblemId",
                table: "ProblemAnswerOption",
                column: "ProblemId",
                principalTable: "Problem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemSet_Lesson_LessonId",
                table: "ProblemSet",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
