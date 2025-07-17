using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace DreamDay.Controllers
{
    [Authorize(Roles = "Planner")]
    public class PlannerReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlannerReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Popular Venues
            var popularVenues = await _context.Weddings
                .GroupBy(w => w.Venue)
                .Select(g => new { Venue = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToListAsync();

            // Average Budgets
            var averageBudget = await _context.BudgetItems
                .GroupBy(b => 1)
                .Select(g => new
                {
                    AverageEstimated = g.Average(b => (decimal?)b.EstimatedCost) ?? 0,
                    AverageActual = g.Average(b => (decimal?)b.ActualCost) ?? 0
                })
                .FirstOrDefaultAsync();

            // Vendor Performance
            var vendorPerformance = await _context.VendorAssignments
                .Include(v => v.Vendor)
                .GroupBy(va => va.Vendor.Name)
                .Select(g => new { VendorName = g.Key, Assignments = g.Count() })
                .OrderByDescending(g => g.Assignments)
                .Take(5)
                .ToListAsync();

            ViewBag.PopularVenues = popularVenues;
            ViewBag.AverageBudget = averageBudget;
            ViewBag.VendorPerformance = vendorPerformance;

            return View();
        }
    }
}
