using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_Managemnt_System.Migrations
{
    public partial class fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ActiveFlag",
                table: "Checkout",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Penalty",
                table: "Checkout",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Describtion",
                table: "Book",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveFlag",
                table: "Checkout");

            migrationBuilder.DropColumn(
                name: "Penalty",
                table: "Checkout");

            migrationBuilder.DropColumn(
                name: "Describtion",
                table: "Book");
        }
    }
}
