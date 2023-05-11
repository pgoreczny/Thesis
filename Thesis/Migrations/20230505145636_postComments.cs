using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class postComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_postComments_postComments_PostCommentid",
                table: "postComments");

            migrationBuilder.DropIndex(
                name: "IX_postComments_PostCommentid",
                table: "postComments");

            migrationBuilder.DropColumn(
                name: "PostCommentid",
                table: "postComments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostCommentid",
                table: "postComments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_postComments_PostCommentid",
                table: "postComments",
                column: "PostCommentid");

            migrationBuilder.AddForeignKey(
                name: "FK_postComments_postComments_PostCommentid",
                table: "postComments",
                column: "PostCommentid",
                principalTable: "postComments",
                principalColumn: "id");
        }
    }
}
