using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    public partial class updatemyaddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MyAddress_IdAccount",
                table: "MyAddress",
                column: "IdAccount");

            migrationBuilder.AddForeignKey(
                name: "FK_MyAddress_Account_IdAccount",
                table: "MyAddress",
                column: "IdAccount",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyAddress_Account_IdAccount",
                table: "MyAddress");

            migrationBuilder.DropIndex(
                name: "IX_MyAddress_IdAccount",
                table: "MyAddress");
        }
    }
}
