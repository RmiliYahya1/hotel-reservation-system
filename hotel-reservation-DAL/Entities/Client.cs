using System.ComponentModel.DataAnnotations;


namespace hotel_reservation_DAL.Entities
{
    public class Client
    {
        public int ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string PhoneNumber { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]

        public string Cin { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        
    }
}
