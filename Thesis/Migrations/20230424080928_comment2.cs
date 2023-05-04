using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class comment2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewComment_AspNetUsers_authorId",
                table: "ReviewComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewComment_answers_AnswerId",
                table: "ReviewComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReviewComment",
                table: "ReviewComment");

            migrationBuilder.RenameTable(
                name: "ReviewComment",
                newName: "reviewComments");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewComment_authorId",
                table: "reviewComments",
                newName: "IX_reviewComments_authorId");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewComment_AnswerId",
                table: "reviewComments",
                newName: "IX_reviewComments_AnswerId");

            migrationBuilder.AlterColumn<string>(
                name: "authorId",
                table: "reviewComments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_reviewComments",
                table: "reviewComments",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_reviewComments_AspNetUsers_authorId",
                table: "reviewComments",
                column: "authorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_reviewComments_answers_AnswerId",
                table: "reviewComments",
                column: "AnswerId",
                principalTable: "answers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reviewComments_AspNetUsers_authorId",
                table: "reviewComments");

            migrationBuilder.DropForeignKey(
                name: "FK_reviewComments_answers_AnswerId",
                table: "reviewComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reviewComments",
                table: "reviewComments");

            migrationBuilder.RenameTable(
                name: "reviewComments",
                newName: "ReviewComment");

            migrationBuilder.RenameIndex(
                name: "IX_reviewComments_authorId",
                table: "ReviewComment",
                newName: "IX_ReviewComment_authorId");

            migrationBuilder.RenameIndex(
                name: "IX_reviewComments_AnswerId",
                table: "ReviewComment",
                newName: "IX_ReviewComment_AnswerId");

            migrationBuilder.AlterColumn<string>(
                name: "authorId",
                table: "ReviewComment",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReviewComment",
                table: "ReviewComment",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewComment_AspNetUsers_authorId",
                table: "ReviewComment",
                column: "authorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewComment_answers_AnswerId",
                table: "ReviewComment",
                column: "AnswerId",
                principalTable: "answers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
