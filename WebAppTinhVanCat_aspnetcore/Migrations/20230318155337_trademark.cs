using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppTinhVanCataspnetcore.Migrations
{
    /// <inheritdoc />
    public partial class trademark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TradeMarkId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TradeMark",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeMark", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_TradeMarkId",
                table: "Product",
                column: "TradeMarkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_TradeMark_TradeMarkId",
                table: "Product",
                column: "TradeMarkId",
                principalTable: "TradeMark",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_TradeMark_TradeMarkId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "TradeMark");

            migrationBuilder.DropIndex(
                name: "IX_Product_TradeMarkId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "TradeMarkId",
                table: "Product");
        }
    }
}
