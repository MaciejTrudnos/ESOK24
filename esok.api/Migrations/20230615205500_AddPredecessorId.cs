using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esok.api.Migrations
{
    public partial class AddPredecessorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PredecessorId",
                table: "Product",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PredecessorId",
                table: "Product");
        }
    }
}
