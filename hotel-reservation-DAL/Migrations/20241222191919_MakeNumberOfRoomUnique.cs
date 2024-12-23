using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotel_reservation_DAL.Migrations
{
    /// <inheritdoc />
    public partial class MakeNumberOfRoomUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // supprimer la colonne ReservationId de la table Payment
            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Payments");
        }
    }
}
