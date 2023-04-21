using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class answers2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answers_files_fileId",
                table: "answers");

            migrationBuilder.AlterColumn<Guid>(
                name: "fileId",
                table: "answers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_answers_files_fileId",
                table: "answers",
                column: "fileId",
                principalTable: "files",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answers_files_fileId",
                table: "answers");

            migrationBuilder.AlterColumn<Guid>(
                name: "fileId",
                table: "answers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_answers_files_fileId",
                table: "answers",
                column: "fileId",
                principalTable: "files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
