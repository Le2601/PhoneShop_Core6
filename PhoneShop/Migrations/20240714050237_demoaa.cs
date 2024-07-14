using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    public partial class demoaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
             name: "Booth_InformationId",
             table: "Product",
             type: "int",
             nullable: false,
             defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
         
        }
    }
}
