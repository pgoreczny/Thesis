using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class answers3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answers_activities_Activityid",
                table: "answers");

            migrationBuilder.RenameColumn(
                name: "Activityid",
                table: "answers",
                newName: "activityId");

            migrationBuilder.RenameIndex(
                name: "IX_answers_Activityid",
                table: "answers",
                newName: "IX_answers_activityId");

            migrationBuilder.AlterColumn<int>(
                name: "activityId",
                table: "answers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_answers_activities_activityId",
                table: "answers",
                column: "activityId",
                principalTable: "activities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answers_activities_activityId",
                table: "answers");

            migrationBuilder.RenameColumn(
                name: "activityId",
                table: "answers",
                newName: "Activityid");

            migrationBuilder.RenameIndex(
                name: "IX_answers_activityId",
                table: "answers",
                newName: "IX_answers_Activityid");

            migrationBuilder.AlterColumn<int>(
                name: "Activityid",
                table: "answers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_answers_activities_Activityid",
                table: "answers",
                column: "Activityid",
                principalTable: "activities",
                principalColumn: "id");
        }
    }
}
