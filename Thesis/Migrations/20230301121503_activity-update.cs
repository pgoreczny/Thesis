using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class activityupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_files_activities_Activityid",
                table: "files");

            migrationBuilder.DropIndex(
                name: "IX_files_Activityid",
                table: "files");

            migrationBuilder.DropColumn(
                name: "Activityid",
                table: "files");

            migrationBuilder.DropColumn(
                name: "containsFile",
                table: "activities");

            migrationBuilder.AddColumn<DateTime>(
                name: "createDate",
                table: "activities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "createdById",
                table: "activities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updateDate",
                table: "activities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "updatedById",
                table: "activities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_activities_createdById",
                table: "activities",
                column: "createdById");

            migrationBuilder.CreateIndex(
                name: "IX_activities_updatedById",
                table: "activities",
                column: "updatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_activities_AspNetUsers_createdById",
                table: "activities",
                column: "createdById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_activities_AspNetUsers_updatedById",
                table: "activities",
                column: "updatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activities_AspNetUsers_createdById",
                table: "activities");

            migrationBuilder.DropForeignKey(
                name: "FK_activities_AspNetUsers_updatedById",
                table: "activities");

            migrationBuilder.DropIndex(
                name: "IX_activities_createdById",
                table: "activities");

            migrationBuilder.DropIndex(
                name: "IX_activities_updatedById",
                table: "activities");

            migrationBuilder.DropColumn(
                name: "createDate",
                table: "activities");

            migrationBuilder.DropColumn(
                name: "createdById",
                table: "activities");

            migrationBuilder.DropColumn(
                name: "updateDate",
                table: "activities");

            migrationBuilder.DropColumn(
                name: "updatedById",
                table: "activities");

            migrationBuilder.AddColumn<int>(
                name: "Activityid",
                table: "files",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "containsFile",
                table: "activities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_files_Activityid",
                table: "files",
                column: "Activityid");

            migrationBuilder.AddForeignKey(
                name: "FK_files_activities_Activityid",
                table: "files",
                column: "Activityid",
                principalTable: "activities",
                principalColumn: "id");
        }
    }
}
