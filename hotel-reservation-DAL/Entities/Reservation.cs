
using System.ComponentModel.DataAnnotations;

namespace hotel_reservation_DAL.Entities
{
    public class Reservation
    {
        public int ID { get; set; }
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        [Required]
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }


    }
}
