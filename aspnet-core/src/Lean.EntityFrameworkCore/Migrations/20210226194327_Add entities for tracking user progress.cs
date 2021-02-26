using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lean.Migrations
{
    public partial class Addentitiesfortrackinguserprogress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLearningProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Step = table.Column<int>(type: "int", nullable: false),
                    CurrentProblemId = table.Column<int>(type: "int", nullable: true),
                    CurrentLessonId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLearningProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLearningProgresses_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLearningProgresses_Lessons_CurrentLessonId",
                        column: x => x.CurrentLessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserLearningProgresses_Problems_CurrentProblemId",
                        column: x => x.CurrentProblemId,
                        principalTable: "Problems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserProblemResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProblemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProblemResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProblemResults_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProblemResults_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserProblemAnswerOptionResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProblemAnswerOptionId = table.Column<int>(type: "int", nullable: false),
                    UserProblemResultId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProblemAnswerOptionResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProblemAnswerOptionResults_ProblemAnswerOptions_ProblemAnswerOptionId",
                        column: x => x.ProblemAnswerOptionId,
                        principalTable: "ProblemAnswerOptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProblemAnswerOptionResults_UserProblemResults_UserProblemResultId",
                        column: x => x.UserProblemResultId,
                        principalTable: "UserProblemResults",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLearningProgresses_CurrentLessonId",
                table: "UserLearningProgresses",
                column: "CurrentLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLearningProgresses_CurrentProblemId",
                table: "UserLearningProgresses",
                column: "CurrentProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLearningProgresses_UserId",
                table: "UserLearningProgresses",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProblemAnswerOptionResults_ProblemAnswerOptionId",
                table: "UserProblemAnswerOptionResults",
                column: "ProblemAnswerOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProblemAnswerOptionResults_UserProblemResultId",
                table: "UserProblemAnswerOptionResults",
                column: "UserProblemResultId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProblemResults_ProblemId",
                table: "UserProblemResults",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProblemResults_UserId",
                table: "UserProblemResults",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLearningProgresses");

            migrationBuilder.DropTable(
                name: "UserProblemAnswerOptionResults");

            migrationBuilder.DropTable(
                name: "UserProblemResults");
        }
    }
}
