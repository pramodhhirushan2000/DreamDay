using System.ComponentModel.DataAnnotations;

namespace DreamDay.Models
{
    public class WeddingTable
    {
        [Key]
        public int TableId { get; set; }

        public int WeddingId { get; set; } // Which wedding this table belongs to
        public string TableName { get; set; } // e.g., "Table 1", "Family Table", etc.
        public int MaxSeats { get; set; } // e.g., 8 seats per table

        public Wedding Wedding { get; set; }
    }
}
