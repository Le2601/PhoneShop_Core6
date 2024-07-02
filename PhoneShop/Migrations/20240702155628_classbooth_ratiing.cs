using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    public partial class classbooth_ratiing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Booth_Information",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Booth_Tracking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity_Product = table.Column<int>(type: "int", nullable: false),
                    Total_Sold = table.Column<int>(type: "int", nullable: false),
                    Followers = table.Column<int>(type: "int", nullable: false),
                    Point_Evaluation = table.Column<double>(type: "float", nullable: false),
                    BoothId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booth_Tracking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booth_Tracking_Booth_Information_BoothId",
                        column: x => x.BoothId,
                        principalTable: "Booth_Information",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booth_Tracking_BoothId",
                table: "Booth_Tracking",
                column: "BoothId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booth_Tracking");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Booth_Information");
        }
    }
}
