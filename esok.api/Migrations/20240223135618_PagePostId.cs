using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esok.api.Migrations
{
    public partial class PagePostId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PagePostId",
                table: "Article",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PagePostId",
                table: "Article");
        }
    }
}
