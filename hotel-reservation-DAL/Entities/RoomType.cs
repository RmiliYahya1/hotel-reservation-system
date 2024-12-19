using System.ComponentModel.DataAnnotations;

namespace hotel_reservation_DAL.Entities
{
    public class RoomType
    {

        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        [Range(1, 4)]
        public int Capacity { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}