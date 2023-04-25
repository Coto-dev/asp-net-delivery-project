using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishDishInCart_Dishes_DishesId",
                table: "DishDishInCart");

            migrationBuilder.RenameColumn(
                name: "DishesId",
                table: "DishDishInCart",
                newName: "DishesCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishDishInCart_Dishes_DishesCartId",
                table: "DishDishInCart",
                column: "DishesCartId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishDishInCart_Dishes_DishesCartId",
                table: "DishDishInCart");

            migrationBuilder.RenameColumn(
                name: "DishesCartId",
                table: "DishDishInCart",
                newName: "DishesId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishDishInCart_Dishes_DishesId",
                table: "DishDishInCart",
                column: "DishesId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
