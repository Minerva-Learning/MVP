using Microsoft.EntityFrameworkCore.Migrations;

namespace Lean.Migrations
{
    public partial class AddFlowRules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlowRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswersCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowRules_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlowRuleNextLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlowRuleId = table.Column<int>(type: "int", nullable: false),
                    NextLessonId = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowRuleNextLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowRuleNextLessons_FlowRules_FlowRuleId",
                        column: x => x.FlowRuleId,
                        principalTable: "FlowRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlowRuleNextLessons_Lessons_NextLessonId",
                        column: x => x.NextLessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FlowRuleProblems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlowRuleId = table.Column<int>(type: "int", nullable: false),
                    ProblemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowRuleProblems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowRuleProblems_FlowRules_FlowRuleId",
                        column: x => x.FlowRuleId,
                        principalTable: "FlowRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlowRuleProblems_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlowRuleNextLessons_FlowRuleId",
                table: "FlowRuleNextLessons",
                column: "FlowRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowRuleNextLessons_NextLessonId",
                table: "FlowRuleNextLessons",
                column: "NextLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowRuleProblems_FlowRuleId",
                table: "FlowRuleProblems",
                column: "FlowRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowRuleProblems_ProblemId",
                table: "FlowRuleProblems",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowRules_LessonId",
                table: "FlowRules",
                column: "LessonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlowRuleNextLessons");

            migrationBuilder.DropTable(
                name: "FlowRuleProblems");

            migrationBuilder.DropTable(
                name: "FlowRules");
        }
    }
}
