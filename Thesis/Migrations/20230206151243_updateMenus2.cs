using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class updateMenus2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_menus_menus_MenuItemId",
                table: "menus");

            migrationBuilder.DropIndex(
                name: "IX_menus_MenuItemId",
                table: "menus");

            migrationBuilder.DropColumn(
                name: "MenuItemId",
                table: "menus");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "menus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_menus_ParentId",
                table: "menus",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_menus_menus_ParentId",
                table: "menus",
                column: "ParentId",
                principalTable: "menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_menus_menus_ParentId",
                table: "menus");

            migrationBuilder.DropIndex(
                name: "IX_menus_ParentId",
                table: "menus");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "menus");

            migrationBuilder.AddColumn<int>(
                name: "MenuItemId",
                table: "menus",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_menus_MenuItemId",
                table: "menus",
                column: "MenuItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_menus_menus_MenuItemId",
                table: "menus",
                column: "MenuItemId",
                principalTable: "menus",
                principalColumn: "Id");
        }
    }
}
