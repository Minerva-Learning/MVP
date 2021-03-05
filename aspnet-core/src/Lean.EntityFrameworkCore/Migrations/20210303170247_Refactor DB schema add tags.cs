using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lean.Migrations
{
    public partial class RefactorDBschemaaddtags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_ProblemSets_ProblemSetId",
                table: "Problems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProblemAnswerOptionResults_UserProblemResults_UserProblemResultId",
                table: "UserProblemAnswerOptionResults");

            migrationBuilder.DropTable(
                name: "ProblemSets");

            migrationBuilder.DropTable(
                name: "UserProblemResults");

            migrationBuilder.RenameColumn(
                name: "ProblemSetId",
                table: "Problems",
                newName: "LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_Problems_ProblemSetId",
                table: "Problems",
                newName: "IX_Problems_LessonId");

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InitialRating = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLessonAnswerSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLessonAnswerSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLessonAnswerSets_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserLessonAnswerSets_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProblemTags",
                columns: table => new
                {
                    ProblemId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    ProblemTagRating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemTags", x => new { x.ProblemId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ProblemTags_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProblemTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProblemAnswerResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    ProblemId = table.Column<int>(type: "int", nullable: false),
                    UserLessonAnswerSetId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProblemAnswerResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProblemAnswerResults_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProblemAnswerResults_UserLessonAnswerSets_UserLessonAnswerSetId",
                        column: x => x.UserLessonAnswerSetId,
                        principalTable: "UserLessonAnswerSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProblemTags_TagId",
                table: "ProblemTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ModuleId",
                table: "Tags",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLessonAnswerSets_LessonId",
                table: "UserLessonAnswerSets",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLessonAnswerSets_UserId",
                table: "UserLessonAnswerSets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProblemAnswerResults_ProblemId",
                table: "UserProblemAnswerResults",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProblemAnswerResults_UserLessonAnswerSetId",
                table: "UserProblemAnswerResults",
                column: "UserLessonAnswerSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_Lessons_LessonId",
                table: "Problems",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProblemAnswerOptionResults_UserProblemAnswerResults_UserProblemResultId",
                table: "UserProblemAnswerOptionResults",
                column: "UserProblemResultId",
                principalTable: "UserProblemAnswerResults",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Lessons_LessonId",
                table: "Problems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProblemAnswerOptionResults_UserProblemAnswerResults_UserProblemResultId",
                table: "UserProblemAnswerOptionResults");

            migrationBuilder.DropTable(
                name: "ProblemTags");

            migrationBuilder.DropTable(
                name: "UserProblemAnswerResults");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "UserLessonAnswerSets");

            migrationBuilder.RenameColumn(
                name: "LessonId",
                table: "Problems",
                newName: "ProblemSetId");

            migrationBuilder.RenameIndex(
                name: "IX_Problems_LessonId",
                table: "Problems",
                newName: "IX_Problems_ProblemSetId");

            migrationBuilder.CreateTable(
                name: "ProblemSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    LessonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProblemSets_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProblemResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    ProblemId = table.Column<int>(type: "int", nullable: false),
                    TextAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_ProblemSets_LessonId",
                table: "ProblemSets",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProblemResults_ProblemId",
                table: "UserProblemResults",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProblemResults_UserId",
                table: "UserProblemResults",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_ProblemSets_ProblemSetId",
                table: "Problems",
                column: "ProblemSetId",
                principalTable: "ProblemSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProblemAnswerOptionResults_UserProblemResults_UserProblemResultId",
                table: "UserProblemAnswerOptionResults",
                column: "UserProblemResultId",
                principalTable: "UserProblemResults",
                principalColumn: "Id");
        }
    }
}
