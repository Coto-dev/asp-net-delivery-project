using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class refactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDishes_Customer_CustomerId",
                table: "CartDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CartDishes_DishesId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customer_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Customer_CustomerId",
                table: "Ratings");

            migrationBuilder.DropTable(
                name: "DishDishInCart");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DishesId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "DishesId",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.AddColumn<Guid>(
                name: "DishId",
                table: "CartDishes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "CartDishes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CartDishes_DishId",
                table: "CartDishes",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_CartDishes_OrderId",
                table: "CartDishes",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDishes_Customers_CustomerId",
                table: "CartDishes",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartDishes_Dishes_DishId",
                table: "CartDishes",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartDishes_Orders_OrderId",
                table: "CartDishes",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Customers_CustomerId",
                table: "Ratings",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDishes_Customers_CustomerId",
                table: "CartDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_CartDishes_Dishes_DishId",
                table: "CartDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_CartDishes_Orders_OrderId",
                table: "CartDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Customers_CustomerId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_CartDishes_DishId",
                table: "CartDishes");

            migrationBuilder.DropIndex(
                name: "IX_CartDishes_OrderId",
                table: "CartDishes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "DishId",
                table: "CartDishes");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "CartDishes");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.AddColumn<Guid>(
                name: "DishesId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DishDishInCart",
                columns: table => new
                {
                    DishesCartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DishesInCartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishDishInCart", x => new { x.DishesCartId, x.DishesInCartId });
                    table.ForeignKey(
                        name: "FK_DishDishInCart_CartDishes_DishesInCartId",
                        column: x => x.DishesInCartId,
                        principalTable: "CartDishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishDishInCart_Dishes_DishesCartId",
                        column: x => x.DishesCartId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DishesId",
                table: "Orders",
                column: "DishesId");

            migrationBuilder.CreateIndex(
                name: "IX_DishDishInCart_DishesInCartId",
                table: "DishDishInCart",
                column: "DishesInCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDishes_Customer_CustomerId",
                table: "CartDishes",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CartDishes_DishesId",
                table: "Orders",
                column: "DishesId",
                principalTable: "CartDishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customer_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Customer_CustomerId",
                table: "Ratings",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
