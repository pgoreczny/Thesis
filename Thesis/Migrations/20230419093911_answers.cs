using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class answers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "path",
                table: "answers");

            migrationBuilder.AddColumn<bool>(
                name: "editable",
                table: "answers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "fileId",
                table: "answers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_answers_fileId",
                table: "answers",
                column: "fileId");

            migrationBuilder.AddForeignKey(
                name: "FK_answers_files_fileId",
                table: "answers",
                column: "fileId",
                principalTable: "files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answers_files_fileId",
                table: "answers");

            migrationBuilder.DropIndex(
                name: "IX_answers_fileId",
                table: "answers");

            migrationBuilder.DropColumn(
                name: "editable",
                table: "answers");

            migrationBuilder.DropColumn(
                name: "fileId",
                table: "answers");

            migrationBuilder.AddColumn<string>(
                name: "path",
                table: "answers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
