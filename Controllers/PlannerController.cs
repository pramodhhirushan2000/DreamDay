using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DreamDay.Controllers
{
    [Authorize(Roles = "Planner")]
    public class PlannerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PlannerController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Dashboard Home
        public async Task<IActionResult> Dashboard()
        {
            var planner = await _userManager.GetUserAsync(User);
            if (planner == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var weddings = await _context.Weddings
                .Where(w => w.PlannerId == planner.Id)
                .ToListAsync();

            var weddingViewModels = new List<WeddingViewModel>();

            foreach (var wedding in weddings)
            {
                var couple = await _userManager.FindByIdAsync(wedding.CoupleId);

                weddingViewModels.Add(new WeddingViewModel
                {
                    WeddingId = wedding.WeddingId,
                    CoupleEmail = couple?.Email ?? "Unknown",
                    WeddingDate = wedding.WeddingDate,
                    Venue = wedding.Venue
                });
            }

            ViewBag.WeddingCount = weddings.Count;
            return View(weddingViewModels);
        }

    }
}
