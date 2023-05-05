using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppTinhVanCataspnetcore.Migrations
{
    /// <inheritdoc />
    public partial class addHomeAdress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HomeAddress",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeAddress",
                table: "Order");
        }
    }
}
