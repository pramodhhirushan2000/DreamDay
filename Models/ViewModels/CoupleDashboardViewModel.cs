using DreamDay.Models;

namespace DreamDay.Models.ViewModels
{
    public class CoupleDashboardViewModel
    {
        public Wedding Wedding { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int TotalGuests { get; set; }
        public int AcceptedGuests { get; set; }
        public decimal EstimatedBudget { get; set; }
        public decimal ActualBudget { get; set; }
    }
}
