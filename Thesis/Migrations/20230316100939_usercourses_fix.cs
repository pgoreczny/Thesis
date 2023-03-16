using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class usercoursesfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseApplicationUser_AspNetUsers_ApplicationUserId",
                table: "CourseApplicationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseApplicationUser_courses_CourseId",
                table: "CourseApplicationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseApplicationUser",
                table: "CourseApplicationUser");

            migrationBuilder.RenameTable(
                name: "CourseApplicationUser",
                newName: "courseApplicationUsers");

            migrationBuilder.RenameIndex(
                name: "IX_CourseApplicationUser_ApplicationUserId",
                table: "courseApplicationUsers",
                newName: "IX_courseApplicationUsers_ApplicationUserId");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "courseApplicationUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_courseApplicationUsers",
                table: "courseApplicationUsers",
                columns: new[] { "CourseId", "ApplicationUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_courseApplicationUsers_AspNetUsers_ApplicationUserId",
                table: "courseApplicationUsers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_courseApplicationUsers_courses_CourseId",
                table: "courseApplicationUsers",
                column: "CourseId",
                principalTable: "courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courseApplicationUsers_AspNetUsers_ApplicationUserId",
                table: "courseApplicationUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_courseApplicationUsers_courses_CourseId",
                table: "courseApplicationUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_courseApplicationUsers",
                table: "courseApplicationUsers");

            migrationBuilder.DropColumn(
                name: "status",
                table: "courseApplicationUsers");

            migrationBuilder.RenameTable(
                name: "courseApplicationUsers",
                newName: "CourseApplicationUser");

            migrationBuilder.RenameIndex(
                name: "IX_courseApplicationUsers_ApplicationUserId",
                table: "CourseApplicationUser",
                newName: "IX_CourseApplicationUser_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseApplicationUser",
                table: "CourseApplicationUser",
                columns: new[] { "CourseId", "ApplicationUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseApplicationUser_AspNetUsers_ApplicationUserId",
                table: "CourseApplicationUser",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseApplicationUser_courses_CourseId",
                table: "CourseApplicationUser",
                column: "CourseId",
                principalTable: "courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
