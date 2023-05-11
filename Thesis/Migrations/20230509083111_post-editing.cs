using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class postediting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "editorId",
                table: "posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "editorId",
                table: "postComments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_posts_editorId",
                table: "posts",
                column: "editorId");

            migrationBuilder.CreateIndex(
                name: "IX_postComments_editorId",
                table: "postComments",
                column: "editorId");

            migrationBuilder.AddForeignKey(
                name: "FK_postComments_AspNetUsers_editorId",
                table: "postComments",
                column: "editorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_posts_AspNetUsers_editorId",
                table: "posts",
                column: "editorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_postComments_AspNetUsers_editorId",
                table: "postComments");

            migrationBuilder.DropForeignKey(
                name: "FK_posts_AspNetUsers_editorId",
                table: "posts");

            migrationBuilder.DropIndex(
                name: "IX_posts_editorId",
                table: "posts");

            migrationBuilder.DropIndex(
                name: "IX_postComments_editorId",
                table: "postComments");

            migrationBuilder.DropColumn(
                name: "editorId",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "editorId",
                table: "postComments");
        }
    }
}
