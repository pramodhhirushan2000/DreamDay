using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamDay.Models
{
    public class BudgetItem
    {
        [Key]
        public int BudgetItemId { get; set; }

        public int WeddingId { get; set; }

        public string Category { get; set; }

        [Precision(18, 2)]
        public decimal EstimatedCost { get; set; }

        [Precision(18, 2)]
        public decimal ActualCost { get; set; }
    }

}