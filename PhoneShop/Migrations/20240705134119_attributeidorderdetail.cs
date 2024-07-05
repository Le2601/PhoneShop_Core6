using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    public partial class attributeidorderdetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
           

            migrationBuilder.AddColumn<int>(
                name: "Order_Detail_Id",
                table: "DeliveryProcess",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        
    }
}
