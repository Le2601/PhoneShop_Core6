using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    public partial class updateclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_ProductPurchasePrice_Order_Details_Order_DetailsId",
                table: "Order_ProductPurchasePrice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order_ProductPurchasePrice",
                table: "Order_ProductPurchasePrice");

            migrationBuilder.DropIndex(
                name: "IX_Order_ProductPurchasePrice_Order_DetailsId",
                table: "Order_ProductPurchasePrice");

            migrationBuilder.DropColumn(
                name: "Order_DetailsId",
                table: "Order_ProductPurchasePrice");

            migrationBuilder.RenameTable(
                name: "Order_ProductPurchasePrice",
                newName: "Order_ProductPurchasePrices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order_ProductPurchasePrices",
                table: "Order_ProductPurchasePrices",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ProductPurchasePrices_OrderDetail_Id",
                table: "Order_ProductPurchasePrices",
                column: "OrderDetail_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_ProductPurchasePrices_Order_Details_OrderDetail_Id",
                table: "Order_ProductPurchasePrices",
                column: "OrderDetail_Id",
                principalTable: "Order_Details",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_ProductPurchasePrices_Order_Details_OrderDetail_Id",
                table: "Order_ProductPurchasePrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order_ProductPurchasePrices",
                table: "Order_ProductPurchasePrices");

            migrationBuilder.DropIndex(
                name: "IX_Order_ProductPurchasePrices_OrderDetail_Id",
                table: "Order_ProductPurchasePrices");

            migrationBuilder.RenameTable(
                name: "Order_ProductPurchasePrices",
                newName: "Order_ProductPurchasePrice");

            migrationBuilder.AddColumn<int>(
                name: "Order_DetailsId",
                table: "Order_ProductPurchasePrice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order_ProductPurchasePrice",
                table: "Order_ProductPurchasePrice",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ProductPurchasePrice_Order_DetailsId",
                table: "Order_ProductPurchasePrice",
                column: "Order_DetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_ProductPurchasePrice_Order_Details_Order_DetailsId",
                table: "Order_ProductPurchasePrice",
                column: "Order_DetailsId",
                principalTable: "Order_Details",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
