﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotel_reservation_DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAmountType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Payments",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");
        }
    }
}
