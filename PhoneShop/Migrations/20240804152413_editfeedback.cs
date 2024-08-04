using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    public partial class editfeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "FeedBackComment",
                newName: "AccountIdFeedBack");

            migrationBuilder.AddColumn<string>(
                name: "UserNameFeedBack",
                table: "FeedBackComment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserNameFeedBack",
                table: "FeedBackComment");

            migrationBuilder.RenameColumn(
                name: "AccountIdFeedBack",
                table: "FeedBackComment",
                newName: "AccountId");
        }
    }
}
