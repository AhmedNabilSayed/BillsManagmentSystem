using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillsDAL.Identity.Migrations
{
    public partial class AddRemeberMeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RemeberMe",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemeberMe",
                table: "AspNetUsers");
        }
    }
}
