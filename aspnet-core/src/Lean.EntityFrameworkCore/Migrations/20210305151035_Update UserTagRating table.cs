using Microsoft.EntityFrameworkCore.Migrations;

namespace Lean.Migrations
{
    public partial class UpdateUserTagRatingtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTagRatings",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTagRatings", x => new { x.TagId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserTagRatings_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserTagRatings_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTagRatings_UserId",
                table: "UserTagRatings",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTagRatings");
        }
    }
}
