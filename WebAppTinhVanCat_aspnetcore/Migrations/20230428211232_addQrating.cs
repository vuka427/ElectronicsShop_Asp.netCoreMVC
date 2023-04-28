using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppTinhVanCataspnetcore.Migrations
{
    /// <inheritdoc />
    public partial class addQrating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "QuantityRating",
                table: "Product",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityRating",
                table: "Product");
        }
    }
}
