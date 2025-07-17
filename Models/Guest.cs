using System.ComponentModel.DataAnnotations;

namespace DreamDay.Models
{
    public class Guest
    {
        [Key]
        public int GuestId { get; set; }
        public int WeddingId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string RSVPStatus { get; set; }
        public string MealPreference { get; set; }

        public int SeatNumber { get; set; } 
        public int? TableId { get; set; }
        public WeddingTable? Table { get; set; }
    }
}
