using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    public partial class relationship_product_114 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluate_Product_Product_Id",
                table: "Evaluate_Product");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Evaluate_Product",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Evaluate_Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluate_Product_ProductId",
                table: "Evaluate_Product",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluate_Product_Product_ProductId",
                table: "Evaluate_Product",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluate_Product_Product_ProductId",
                table: "Evaluate_Product");

            migrationBuilder.DropIndex(
                name: "IX_Evaluate_Product_ProductId",
                table: "Evaluate_Product");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Evaluate_Product");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Evaluate_Product",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluate_Product_Product_Id",
                table: "Evaluate_Product",
                column: "Id",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
