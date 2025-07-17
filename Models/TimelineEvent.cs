using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamDay.Models
{
    public class TimelineEvent
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        public int WeddingId { get; set; } // FK to Wedding

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime EventTime { get; set; } // Required event datetime

        public string Description { get; set; }

        public int? VendorId { get; set; } // Optional vendor reference

        public DateTime? VendorArrivalTime { get; set; } // Optional arrival

        [ForeignKey("VendorId")]
        public Vendor? Vendor { get; set; }

        [ForeignKey("WeddingId")]
        public Wedding? Wedding { get; set; }
    }
}
