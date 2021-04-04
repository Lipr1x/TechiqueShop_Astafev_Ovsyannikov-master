using Microsoft.EntityFrameworkCore.Migrations;

namespace TechiqueShopDatabaseImplement.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "DeliveryComponents");

            migrationBuilder.AlterColumn<string>(
                name: "SupplyName",
                table: "Supplies",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Deliveries",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Deliveries");

            migrationBuilder.AlterColumn<int>(
                name: "SupplyName",
                table: "Supplies",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "DeliveryComponents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
