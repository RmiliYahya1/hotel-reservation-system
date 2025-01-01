using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace hotel_reservation_DAL.Entities
{
    public class RoomType
    {

        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        [Range(1, 4)]
        public int Capacity { get; set; }
        public virtual List<Room> Rooms { get; set; }
        
    }
}