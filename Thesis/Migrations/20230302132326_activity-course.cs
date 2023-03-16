using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class activitycourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activities_courses_Courseid",
                table: "activities");

            migrationBuilder.RenameColumn(
                name: "Courseid",
                table: "activities",
                newName: "courseid");

            migrationBuilder.RenameIndex(
                name: "IX_activities_Courseid",
                table: "activities",
                newName: "IX_activities_courseid");

            migrationBuilder.AlterColumn<int>(
                name: "courseid",
                table: "activities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_activities_courses_courseid",
                table: "activities",
                column: "courseid",
                principalTable: "courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activities_courses_courseid",
                table: "activities");

            migrationBuilder.RenameColumn(
                name: "courseid",
                table: "activities",
                newName: "Courseid");

            migrationBuilder.RenameIndex(
                name: "IX_activities_courseid",
                table: "activities",
                newName: "IX_activities_Courseid");

            migrationBuilder.AlterColumn<int>(
                name: "Courseid",
                table: "activities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_activities_courses_Courseid",
                table: "activities",
                column: "Courseid",
                principalTable: "courses",
                principalColumn: "id");
        }
    }
}
