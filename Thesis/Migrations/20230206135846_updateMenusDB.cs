using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Migrations
{
    /// <inheritdoc />
    public partial class updateMenusDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
