using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillsDAL.Identity.Migrations
{
    public partial class AddImageProfileColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<string>(
                name: "ImageProfile",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageProfile",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RememberMe",
                table: "AspNetUsers",
                newName: "RemeberMe");
        }
    }
}
