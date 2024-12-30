using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_reservation_DAL.Entities
{
    public class Payment
    {

        public int ID { get; set; }
        [Required]
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public virtual Reservation Reservation { get; set; }
        public string PaymentMethod { get; set; }
    }
}
