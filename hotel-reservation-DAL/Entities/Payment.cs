using System.ComponentModel.DataAnnotations;

namespace hotel_reservation_DAL.Entities
{
    public class Payment
    {

        public int ID { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public virtual Reservation Reservation { get; set; }
        public string PaymentMethod { get; set; }
    }
}
