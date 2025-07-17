using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace DreamDay.Controllers
{
    [Authorize(Roles = "Planner")]
    public class PlannerChecklistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlannerChecklistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // View Checklist
        public async Task<IActionResult> Index(int weddingId)
        {
            var checklistItems = await _context.ChecklistItems
                .Include(c => c.Vendor)
                .Where(c => c.WeddingId == weddingId)
                .ToListAsync();

            ViewBag.WeddingId = weddingId;
            return View("~/Views/ChecklistManagement/Index.cshtml", checklistItems);
        }

        // Mark Complete
        public async Task<IActionResult> MarkComplete(int id, int weddingId)
        {
            var item = await _context.ChecklistItems.FindAsync(id);

            if (item != null)
            {
                item.IsCompleted = true;
                _context.Update(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { weddingId });
        }
    }
}
