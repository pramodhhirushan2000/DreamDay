using System;
using System.ComponentModel.DataAnnotations;

namespace DreamDay.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId { get; set; }
        public string CoupleId { get; set; }
        public string Title { get; set; }
        public DateTime WeddingDate { get; set; }
        public string Venue { get; set; }
        public string? PlannerId { get; set; }
    }
}
