using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    public partial class addIdVoucherCLassshiing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoucherId",
                table: "ShippingFees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShippingFees_VoucherId",
                table: "ShippingFees",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingFees_Voucher_VoucherId",
                table: "ShippingFees",
                column: "VoucherId",
                principalTable: "Voucher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingFees_Voucher_VoucherId",
                table: "ShippingFees");

            migrationBuilder.DropIndex(
                name: "IX_ShippingFees_VoucherId",
                table: "ShippingFees");

            migrationBuilder.DropColumn(
                name: "VoucherId",
                table: "ShippingFees");
        }
    }
}
