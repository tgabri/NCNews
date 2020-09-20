using Microsoft.EntityFrameworkCore.Migrations;

namespace NCNews.API.Data.Migrations
{
    public partial class DropForeignKeyAuthorId1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_AspNetUsers_AuthorId1",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_AuthorId1",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "AuthorId1",
                table: "Articles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId1",
                table: "Articles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorId1",
                table: "Articles",
                column: "AuthorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_AuthorId1",
                table: "Articles",
                column: "AuthorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
