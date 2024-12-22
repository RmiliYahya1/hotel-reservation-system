using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotel_reservation_DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbShema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //changer le type de la colonne Amount de decimal à double
            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Payments",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");
            //changer le type de la colonne Price de decimal à double
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Reservations",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");
            //changer le type de la colonne Price de decimal à double
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "RoomTypes",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

        }
    }
}
