using Microsoft.EntityFrameworkCore.Migrations;

namespace TechiqueShopDatabaseImplement.Migrations
{
    public partial class LinkProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Assemblys",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Assemblys");
        }
    }
}
