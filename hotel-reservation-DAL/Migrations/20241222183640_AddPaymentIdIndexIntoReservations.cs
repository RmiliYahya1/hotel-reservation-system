using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotel_reservation_DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentIdIndexIntoReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //mettre la colonne PaymentId dans Reservation comme index
            migrationBuilder.CreateIndex(
                name: "IX_Reservations_PaymentId",
                table: "Reservations",
                column: "PaymentId");
        }
    }
}
