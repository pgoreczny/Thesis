using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class postComments3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_postComments_posts_Postid",
                table: "postComments");

            migrationBuilder.RenameColumn(
                name: "Postid",
                table: "postComments",
                newName: "postId");

            migrationBuilder.RenameIndex(
                name: "IX_postComments_Postid",
                table: "postComments",
                newName: "IX_postComments_postId");

            migrationBuilder.AddForeignKey(
                name: "FK_postComments_posts_postId",
                table: "postComments",
                column: "postId",
                principalTable: "posts",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_postComments_posts_postId",
                table: "postComments");

            migrationBuilder.RenameColumn(
                name: "postId",
                table: "postComments",
                newName: "Postid");

            migrationBuilder.RenameIndex(
                name: "IX_postComments_postId",
                table: "postComments",
                newName: "IX_postComments_Postid");

            migrationBuilder.AddForeignKey(
                name: "FK_postComments_posts_Postid",
                table: "postComments",
                column: "Postid",
                principalTable: "posts",
                principalColumn: "id");
        }
    }
}
