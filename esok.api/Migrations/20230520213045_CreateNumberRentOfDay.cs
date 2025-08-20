using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esok.api.Migrations
{
    public partial class CreateNumberRentOfDay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberRentOfDay",
                table: "RentFinished",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberRentOfDay",
                table: "Rent",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberRentOfDay",
                table: "RentFinished");

            migrationBuilder.DropColumn(
                name: "NumberRentOfDay",
                table: "Rent");
        }
    }
}
