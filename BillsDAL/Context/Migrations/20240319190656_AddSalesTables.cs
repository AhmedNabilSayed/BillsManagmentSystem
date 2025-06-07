using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillsDAL.Migrations
{
    public partial class AddSalesTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BILPRC",
                table: "BillHeaders",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "SalesBillHeaders",
                columns: table => new
                {
                    BILCOD = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BILDAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    BILPRC = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BILIMG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesBillHeaders", x => x.BILCOD);
                    table.ForeignKey(
                        name: "FK_SalesBillHeaders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesBillHeaders_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesBillDetails",
                columns: table => new
                {
                    DTLCOD = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ITMPRC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ITMQTY = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    BILCOD = table.Column<int>(type: "int", nullable: false),
                    ITMCOD = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesBillDetails", x => x.DTLCOD);
                    table.ForeignKey(
                        name: "FK_SalesBillDetails_Items_ITMCOD",
                        column: x => x.ITMCOD,
                        principalTable: "Items",
                        principalColumn: "ItmCod",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesBillDetails_SalesBillHeaders_BILCOD",
                        column: x => x.BILCOD,
                        principalTable: "SalesBillHeaders",
                        principalColumn: "BILCOD",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesBillDetails_BILCOD",
                table: "SalesBillDetails",
                column: "BILCOD");

            migrationBuilder.CreateIndex(
                name: "IX_SalesBillDetails_ITMCOD",
                table: "SalesBillDetails",
                column: "ITMCOD");

            migrationBuilder.CreateIndex(
                name: "IX_SalesBillHeaders_CustomerId",
                table: "SalesBillHeaders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesBillHeaders_StoreId",
                table: "SalesBillHeaders",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesBillDetails");

            migrationBuilder.DropTable(
                name: "SalesBillHeaders");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.AlterColumn<decimal>(
                name: "BILPRC",
                table: "BillHeaders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
