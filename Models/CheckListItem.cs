using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamDay.Models
{
    public class ChecklistItem
    {
        [Key]
        public int ItemId { get; set; }
        public int WeddingId { get; set; }
        public string? Title { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }

        public string? VendorCategory { get; set; }  // nullable string
        public int? VendorId { get; set; }           // nullable int

        [ForeignKey("VendorId")]
        public Vendor? Vendor { get; set; }           // navigation property
    }
}