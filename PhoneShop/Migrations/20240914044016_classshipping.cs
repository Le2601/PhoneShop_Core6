using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    public partial class classshipping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voucher_Booth_Information_BoothId",
                table: "Voucher");

            migrationBuilder.AlterColumn<int>(
                name: "BoothId",
                table: "Voucher",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ShippingFees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FeeMount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingFees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingFees_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id_Order",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShippingFees_OrderId",
                table: "ShippingFees",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Voucher_Booth_Information_BoothId",
                table: "Voucher",
                column: "BoothId",
                principalTable: "Booth_Information",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voucher_Booth_Information_BoothId",
                table: "Voucher");

            migrationBuilder.DropTable(
                name: "ShippingFees");

            migrationBuilder.AlterColumn<int>(
                name: "BoothId",
                table: "Voucher",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Voucher_Booth_Information_BoothId",
                table: "Voucher",
                column: "BoothId",
                principalTable: "Booth_Information",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
