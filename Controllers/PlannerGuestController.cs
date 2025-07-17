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
    public class PlannerGuestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlannerGuestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int weddingId)
        {
            var guests = await _context.Guests
                .Where(g => g.WeddingId == weddingId)
                .ToListAsync();

            ViewBag.WeddingId = weddingId;
            return View(guests);
        }
    }
}
