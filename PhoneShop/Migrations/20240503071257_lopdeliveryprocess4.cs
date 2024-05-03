using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    public partial class lopdeliveryprocess4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Order_Id",
                table: "DeliveryProcess",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryProcess_Order_Id",
                table: "DeliveryProcess",
                column: "Order_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryProcess_Order_Order_Id",
                table: "DeliveryProcess",
                column: "Order_Id",
                principalTable: "Order",
                principalColumn: "Id_Order",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryProcess_Order_Order_Id",
                table: "DeliveryProcess");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryProcess_Order_Id",
                table: "DeliveryProcess");

            migrationBuilder.AlterColumn<string>(
                name: "Order_Id",
                table: "DeliveryProcess",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
