using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneShop.Migrations
{
    public partial class thaydoidbquesprodct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBackComment_ProductQuestions_RwProductId",
                table: "FeedBackComment");

            migrationBuilder.RenameColumn(
                name: "RwProductId",
                table: "FeedBackComment",
                newName: "QuesProductId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedBackComment_RwProductId",
                table: "FeedBackComment",
                newName: "IX_FeedBackComment_QuesProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBackComment_ProductQuestions_QuesProductId",
                table: "FeedBackComment",
                column: "QuesProductId",
                principalTable: "ProductQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBackComment_ProductQuestions_QuesProductId",
                table: "FeedBackComment");

            migrationBuilder.RenameColumn(
                name: "QuesProductId",
                table: "FeedBackComment",
                newName: "RwProductId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedBackComment_QuesProductId",
                table: "FeedBackComment",
                newName: "IX_FeedBackComment_RwProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBackComment_ProductQuestions_RwProductId",
                table: "FeedBackComment",
                column: "RwProductId",
                principalTable: "ProductQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
