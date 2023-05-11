using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class posts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    activityId = table.Column<int>(type: "int", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    authorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    edited = table.Column<bool>(type: "bit", nullable: false),
                    editDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posts", x => x.id);
                    table.ForeignKey(
                        name: "FK_posts_AspNetUsers_authorId",
                        column: x => x.authorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_posts_activities_activityId",
                        column: x => x.activityId,
                        principalTable: "activities",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "postComments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    authorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    edited = table.Column<bool>(type: "bit", nullable: false),
                    editDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostCommentid = table.Column<int>(type: "int", nullable: true),
                    Postid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_postComments", x => x.id);
                    table.ForeignKey(
                        name: "FK_postComments_AspNetUsers_authorId",
                        column: x => x.authorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_postComments_postComments_PostCommentid",
                        column: x => x.PostCommentid,
                        principalTable: "postComments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_postComments_posts_Postid",
                        column: x => x.Postid,
                        principalTable: "posts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_postComments_authorId",
                table: "postComments",
                column: "authorId");

            migrationBuilder.CreateIndex(
                name: "IX_postComments_PostCommentid",
                table: "postComments",
                column: "PostCommentid");

            migrationBuilder.CreateIndex(
                name: "IX_postComments_Postid",
                table: "postComments",
                column: "Postid");

            migrationBuilder.CreateIndex(
                name: "IX_posts_activityId",
                table: "posts",
                column: "activityId");

            migrationBuilder.CreateIndex(
                name: "IX_posts_authorId",
                table: "posts",
                column: "authorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "postComments");

            migrationBuilder.DropTable(
                name: "posts");
        }
    }
}
