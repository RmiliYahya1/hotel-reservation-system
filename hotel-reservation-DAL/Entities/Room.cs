using System.ComponentModel.DataAnnotations;

namespace hotel_reservation_DAL.Entities
{
    public class Room
    {

        public int ID { get; set; }
        public required string Number { get; set; }
        public bool IsAvailable { get; set; }
        public int RoomTypeId { get; set; }
        public virtual  RoomType RoomType { get; set; }
        public virtual  ICollection<Reservation> Reservations { get; set; }

    }
}