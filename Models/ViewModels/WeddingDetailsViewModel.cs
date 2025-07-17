using DreamDay.Models;
using System.Collections.Generic;

namespace DreamDay.Models.ViewModels
{
    public class WeddingDetailsViewModel
    {
        public Wedding Wedding { get; set; }
        public List<ChecklistItem> ChecklistItems { get; set; }
        public List<Guest> Guests { get; set; }
        public List<BudgetItem> BudgetItems { get; set; }
    }
}
