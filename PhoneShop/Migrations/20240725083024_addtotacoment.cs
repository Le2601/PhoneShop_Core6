using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    public partial class addtotacoment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Total_Comments",
                table: "Booth_Tracking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_Booth_InformationId",
                table: "Product",
                column: "Booth_InformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Booth_Information_Booth_InformationId",
                table: "Product",
                column: "Booth_InformationId",
                principalTable: "Booth_Information",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Booth_Information_Booth_InformationId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_Booth_InformationId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Total_Comments",
                table: "Booth_Tracking");
        }
    }
}
