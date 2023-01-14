using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppTinhVanCataspnetcore.Migrations
{
    /// <inheritdoc />
    public partial class editOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Users_CustomerId",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Order",
                newName: "CustomerID");

            migrationBuilder.RenameColumn(
                name: "Adress",
                table: "Order",
                newName: "TransportCode");

            migrationBuilder.RenameIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                newName: "IX_Order_CustomerID");

            migrationBuilder.AddColumn<string>(
                name: "Feature",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderItemID",
                table: "OrderItem",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "GTGT",
                table: "OrderItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductTitle",
                table: "OrderItem",
                type: "nvarchar(160)",
                maxLength: 160,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CustomNote",
                table: "Order",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShopNote",
                table: "Order",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem",
                column: "OrderItemID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ProductID",
                table: "OrderItem",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Users_CustomerID",
                table: "Order",
                column: "CustomerID",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Users_CustomerID",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem");

            migrationBuilder.DropIndex(
                name: "IX_OrderItem_ProductID",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "Feature",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "OrderItemID",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "GTGT",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "ProductTitle",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ShopNote",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "Order",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "TransportCode",
                table: "Order",
                newName: "Adress");

            migrationBuilder.RenameIndex(
                name: "IX_Order_CustomerID",
                table: "Order",
                newName: "IX_Order_CustomerId");

            migrationBuilder.AlterColumn<string>(
                name: "CustomNote",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem",
                columns: new[] { "ProductID", "OrderID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Users_CustomerId",
                table: "Order",
                column: "CustomerId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
