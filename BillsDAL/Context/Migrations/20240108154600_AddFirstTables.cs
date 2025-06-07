using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillsDAL.Migrations
{
    public partial class AddFirstTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItmCod = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItmNam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItmPrc = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItmCod);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    VndCod = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VndNam = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.VndCod);
                });

            migrationBuilder.CreateTable(
                name: "BillHeaders",
                columns: table => new
                {
                    BILCOD = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BILDAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VNDCOD = table.Column<int>(type: "int", nullable: false),
                    BILPRC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BILIMG = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillHeaders", x => x.BILCOD);
                    table.ForeignKey(
                        name: "FK_BillHeaders_Vendors_VNDCOD",
                        column: x => x.VNDCOD,
                        principalTable: "Vendors",
                        principalColumn: "VndCod",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillDetails",
                columns: table => new
                {
                    DTLCOD = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BILCOD = table.Column<int>(type: "int", nullable: false),
                    ITMCOD = table.Column<int>(type: "int", nullable: false),
                    ITMPRC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ITMQTY = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillDetails", x => x.DTLCOD);
                    table.ForeignKey(
                        name: "FK_BillDetails_BillHeaders_BILCOD",
                        column: x => x.BILCOD,
                        principalTable: "BillHeaders",
                        principalColumn: "BILCOD",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillDetails_Items_ITMCOD",
                        column: x => x.ITMCOD,
                        principalTable: "Items",
                        principalColumn: "ItmCod",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_BILCOD",
                table: "BillDetails",
                column: "BILCOD");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_ITMCOD",
                table: "BillDetails",
                column: "ITMCOD");

            migrationBuilder.CreateIndex(
                name: "IX_BillHeaders_VNDCOD",
                table: "BillHeaders",
                column: "VNDCOD");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillDetails");

            migrationBuilder.DropTable(
                name: "BillHeaders");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Vendors");
        }
    }
}
