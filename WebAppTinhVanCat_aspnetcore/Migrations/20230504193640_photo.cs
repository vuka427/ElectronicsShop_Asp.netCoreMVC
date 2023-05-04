using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppTinhVanCataspnetcore.Migrations
{
    /// <inheritdoc />
    public partial class photo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoName",
                table: "Post",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoName",
                table: "Post");
        }
    }
}
