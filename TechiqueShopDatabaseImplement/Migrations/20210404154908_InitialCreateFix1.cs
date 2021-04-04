using Microsoft.EntityFrameworkCore.Migrations;

namespace TechiqueShopDatabaseImplement.Migrations
{
    public partial class InitialCreateFix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Deliveries");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "DeliveryComponents",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "DeliveryComponents");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
