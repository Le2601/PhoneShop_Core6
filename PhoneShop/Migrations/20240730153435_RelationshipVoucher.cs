using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    public partial class RelationshipVoucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Voucher_BoothId",
                table: "Voucher",
                column: "BoothId");

            migrationBuilder.AddForeignKey(
                name: "FK_Voucher_Booth_Information_BoothId",
                table: "Voucher",
                column: "BoothId",
                principalTable: "Booth_Information",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voucher_Booth_Information_BoothId",
                table: "Voucher");

            migrationBuilder.DropIndex(
                name: "IX_Voucher_BoothId",
                table: "Voucher");
        }
    }
}
