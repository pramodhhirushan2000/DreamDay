using System;

namespace DreamDay.Models
{
    public class VendorAssignment
    {
        public int VendorAssignmentId { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public int WeddingId { get; set; }
        public Wedding Wedding { get; set; }

        public DateTime AssignedDate { get; set; } = DateTime.Now;
    }
}
