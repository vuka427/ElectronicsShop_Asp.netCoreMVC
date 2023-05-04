﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppTinhVanCataspnetcore.Migrations
{
    /// <inheritdoc />
    public partial class sold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sold",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sold",
                table: "Product");
        }
    }
}
