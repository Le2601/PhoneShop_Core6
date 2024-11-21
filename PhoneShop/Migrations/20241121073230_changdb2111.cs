using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    public partial class changdb2111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScoreEvaluation",
                table: "Evaluate_Product");

            migrationBuilder.AddColumn<bool>(
                name: "IsOutstanding",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOutstanding",
                table: "Product");

            migrationBuilder.AddColumn<double>(
                name: "ScoreEvaluation",
                table: "Evaluate_Product",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
