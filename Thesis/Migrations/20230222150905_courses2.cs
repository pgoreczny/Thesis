using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class courses2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "createDate",
                table: "courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "createdById",
                table: "courses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "endDate",
                table: "courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "startDate",
                table: "courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updateDate",
                table: "courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "updatedById",
                table: "courses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_courses_createdById",
                table: "courses",
                column: "createdById");

            migrationBuilder.CreateIndex(
                name: "IX_courses_updatedById",
                table: "courses",
                column: "updatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_courses_AspNetUsers_createdById",
                table: "courses",
                column: "createdById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_courses_AspNetUsers_updatedById",
                table: "courses",
                column: "updatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courses_AspNetUsers_createdById",
                table: "courses");

            migrationBuilder.DropForeignKey(
                name: "FK_courses_AspNetUsers_updatedById",
                table: "courses");

            migrationBuilder.DropIndex(
                name: "IX_courses_createdById",
                table: "courses");

            migrationBuilder.DropIndex(
                name: "IX_courses_updatedById",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "createDate",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "createdById",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "endDate",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "startDate",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "updateDate",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "updatedById",
                table: "courses");
        }
    }
}
