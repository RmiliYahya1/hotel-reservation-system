using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotel_reservation_DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateStatusProperty2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Reservations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Reservations",
                type: "longtext",
                nullable: false,
                computedColumnSql: "CASE WHEN PaymentId IS NOT NULL THEN 'Paid' ELSE 'Not Paid' END")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
