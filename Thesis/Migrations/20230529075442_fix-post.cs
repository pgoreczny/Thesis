using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class fixpost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "courseId",
                table: "posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_posts_courseId",
                table: "posts",
                column: "courseId");

            migrationBuilder.AddForeignKey(
                name: "FK_posts_courses_courseId",
                table: "posts",
                column: "courseId",
                principalTable: "courses",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_posts_courses_courseId",
                table: "posts");

            migrationBuilder.DropIndex(
                name: "IX_posts_courseId",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "courseId",
                table: "posts");
        }
    }
}
