using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class taskscleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answers_activities_Taskid",
                table: "answers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "activities");

            migrationBuilder.RenameColumn(
                name: "Taskid",
                table: "answers",
                newName: "Activityid");

            migrationBuilder.RenameIndex(
                name: "IX_answers_Taskid",
                table: "answers",
                newName: "IX_answers_Activityid");

            migrationBuilder.AddForeignKey(
                name: "FK_answers_activities_Activityid",
                table: "answers",
                column: "Activityid",
                principalTable: "activities",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answers_activities_Activityid",
                table: "answers");

            migrationBuilder.RenameColumn(
                name: "Activityid",
                table: "answers",
                newName: "Taskid");

            migrationBuilder.RenameIndex(
                name: "IX_answers_Activityid",
                table: "answers",
                newName: "IX_answers_Taskid");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "activities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_answers_activities_Taskid",
                table: "answers",
                column: "Taskid",
                principalTable: "activities",
                principalColumn: "id");
        }
    }
}
