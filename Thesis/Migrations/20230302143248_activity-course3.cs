using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class activitycourse3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activities_courses_courseid",
                table: "activities");

            migrationBuilder.RenameColumn(
                name: "courseid",
                table: "activities",
                newName: "courseId");

            migrationBuilder.RenameIndex(
                name: "IX_activities_courseid",
                table: "activities",
                newName: "IX_activities_courseId");

            migrationBuilder.AddForeignKey(
                name: "FK_activities_courses_courseId",
                table: "activities",
                column: "courseId",
                principalTable: "courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activities_courses_courseId",
                table: "activities");

            migrationBuilder.RenameColumn(
                name: "courseId",
                table: "activities",
                newName: "courseid");

            migrationBuilder.RenameIndex(
                name: "IX_activities_courseId",
                table: "activities",
                newName: "IX_activities_courseid");

            migrationBuilder.AddForeignKey(
                name: "FK_activities_courses_courseid",
                table: "activities",
                column: "courseid",
                principalTable: "courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
