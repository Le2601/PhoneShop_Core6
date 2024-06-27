using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    public partial class quanlygianhang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Booth_Information",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creare_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booth_Information", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bank_Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CMND = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name_Bank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bank_Account_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bank_Account_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BoothId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bank_Account_Booth_Information_BoothId",
                        column: x => x.BoothId,
                        principalTable: "Booth_Information",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shipping_Method",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COD = table.Column<int>(type: "int", nullable: false),
                    Online_Payment = table.Column<int>(type: "int", nullable: false),
                    BoothId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipping_Method", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shipping_Method_Booth_Information_BoothId",
                        column: x => x.BoothId,
                        principalTable: "Booth_Information",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_Detail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BoothId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopAddress_Booth_Information_BoothId",
                        column: x => x.BoothId,
                        principalTable: "Booth_Information",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bank_Account_BoothId",
                table: "Bank_Account",
                column: "BoothId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipping_Method_BoothId",
                table: "Shipping_Method",
                column: "BoothId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopAddress_BoothId",
                table: "ShopAddress",
                column: "BoothId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bank_Account");

            migrationBuilder.DropTable(
                name: "Shipping_Method");

            migrationBuilder.DropTable(
                name: "ShopAddress");

            migrationBuilder.DropTable(
                name: "Booth_Information");
        }
    }
}
