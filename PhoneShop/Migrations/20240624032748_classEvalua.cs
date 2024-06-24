using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    public partial class classEvalua : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluate_Product_Account_AccountId",
                table: "Evaluate_Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Evaluate_Product_Product_ProductId1",
                table: "Evaluate_Product");

            migrationBuilder.DropIndex(
                name: "IX_Evaluate_Product_ProductId",
                table: "Evaluate_Product");

            migrationBuilder.DropIndex(
                name: "IX_Evaluate_Product_ProductId1",
                table: "Evaluate_Product");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "Evaluate_Product");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Evaluate_Product",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluate_Product_ProductId",
                table: "Evaluate_Product",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluate_Product_Account_AccountId",
                table: "Evaluate_Product",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluate_Product_Account_AccountId",
                table: "Evaluate_Product");

            migrationBuilder.DropIndex(
                name: "IX_Evaluate_Product_ProductId",
                table: "Evaluate_Product");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Evaluate_Product",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                table: "Evaluate_Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluate_Product_ProductId",
                table: "Evaluate_Product",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluate_Product_ProductId1",
                table: "Evaluate_Product",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluate_Product_Account_AccountId",
                table: "Evaluate_Product",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluate_Product_Product_ProductId1",
                table: "Evaluate_Product",
                column: "ProductId1",
                principalTable: "Product",
                principalColumn: "Id");
        }
    }
}
