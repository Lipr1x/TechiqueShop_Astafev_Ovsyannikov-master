using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechiqueShopDatabaseImplement.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(nullable: false),
                    CustomerSurname = table.Column<string>(nullable: false),
                    Patronymic = table.Column<string>(nullable: false),
                    Telephone = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    UserType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderName = table.Column<string>(nullable: false),
                    ProviderSurname = table.Column<string>(nullable: false),
                    Patronymic = table.Column<string>(nullable: false),
                    Telephone = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    UserType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GetTechniquies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    ArrivalTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetTechniquies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GetTechniquies_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    OrderName = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Supplies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    SupplyName = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplies_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assemblys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderId = table.Column<int>(nullable: false),
                    AssemblyName = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assemblys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assemblys_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderId = table.Column<int>(nullable: false),
                    ComponentName = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Components_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deliveries_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplyGetTechniques",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplyId = table.Column<int>(nullable: false),
                    GetTechniqueId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyGetTechniques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplyGetTechniques_GetTechniquies_GetTechniqueId",
                        column: x => x.GetTechniqueId,
                        principalTable: "GetTechniquies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplyGetTechniques_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplyOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplyId = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    Sum = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplyOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplyOrders_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssemblyOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssemblyId = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssemblyOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssemblyOrders_Assemblys_AssemblyId",
                        column: x => x.AssemblyId,
                        principalTable: "Assemblys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssemblyOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssemblyComponents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssemblyId = table.Column<int>(nullable: false),
                    ComponentId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssemblyComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssemblyComponents_Assemblys_AssemblyId",
                        column: x => x.AssemblyId,
                        principalTable: "Assemblys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssemblyComponents_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplyComponents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplyId = table.Column<int>(nullable: false),
                    ComponentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplyComponents_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplyComponents_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryComponents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryId = table.Column<int>(nullable: false),
                    ComponentId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryComponents_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryComponents_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyComponents_AssemblyId",
                table: "AssemblyComponents",
                column: "AssemblyId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyComponents_ComponentId",
                table: "AssemblyComponents",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyOrders_AssemblyId",
                table: "AssemblyOrders",
                column: "AssemblyId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyOrders_OrderId",
                table: "AssemblyOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Assemblys_ProviderId",
                table: "Assemblys",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Components_ProviderId",
                table: "Components",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_ProviderId",
                table: "Deliveries",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryComponents_ComponentId",
                table: "DeliveryComponents",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryComponents_DeliveryId",
                table: "DeliveryComponents",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_GetTechniquies_CustomerId",
                table: "GetTechniquies",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_CustomerId",
                table: "Supplies",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyComponents_ComponentId",
                table: "SupplyComponents",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyComponents_SupplyId",
                table: "SupplyComponents",
                column: "SupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyGetTechniques_GetTechniqueId",
                table: "SupplyGetTechniques",
                column: "GetTechniqueId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyGetTechniques_SupplyId",
                table: "SupplyGetTechniques",
                column: "SupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyOrders_OrderId",
                table: "SupplyOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyOrders_SupplyId",
                table: "SupplyOrders",
                column: "SupplyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssemblyComponents");

            migrationBuilder.DropTable(
                name: "AssemblyOrders");

            migrationBuilder.DropTable(
                name: "DeliveryComponents");

            migrationBuilder.DropTable(
                name: "SupplyComponents");

            migrationBuilder.DropTable(
                name: "SupplyGetTechniques");

            migrationBuilder.DropTable(
                name: "SupplyOrders");

            migrationBuilder.DropTable(
                name: "Assemblys");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "GetTechniquies");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Supplies");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
