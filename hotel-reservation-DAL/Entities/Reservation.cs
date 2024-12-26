
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hotel_reservation_DAL.Entities
{  
  public class Reservation
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public DateTime CheckInDate { get; set; }  
        public DateTime CheckOutDate { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
        public double Price { get; set; }
        public int? PaymentId { get; set; }
        public virtual Payment Payment { get; set; }
        public bool IsPaid => PaymentId != null;
    }
}
