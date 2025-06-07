using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillsDAL.Migrations
{
    public partial class AddRelationShipeBetweenBillHeaderAndStoreTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "BillHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BillHeaders_StoreId",
                table: "BillHeaders",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_BillHeaders_Stores_StoreId",
                table: "BillHeaders",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillHeaders_Stores_StoreId",
                table: "BillHeaders");

            migrationBuilder.DropIndex(
                name: "IX_BillHeaders_StoreId",
                table: "BillHeaders");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "BillHeaders");
        }
    }
}
