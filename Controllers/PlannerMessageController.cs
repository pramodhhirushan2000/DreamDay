using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace DreamDay.Controllers
{
    [Authorize(Roles = "Planner")]
    public class PlannerMessageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PlannerMessageController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int weddingId)
        {
            var messages = await _context.Messages
                .Where(m => m.WeddingId == weddingId)
                .OrderBy(m => m.SentDate)
                .ToListAsync();

            ViewBag.WeddingId = weddingId;
            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(int weddingId, string content)
        {
            var planner = await _userManager.GetUserAsync(User);
            var wedding = await _context.Weddings.FindAsync(weddingId);

            if (wedding == null || planner == null)
            {
                return NotFound();
            }

            var message = new Message
            {
                SenderId = planner.Id,
                ReceiverId = wedding.CoupleId,
                WeddingId = weddingId,
                Content = content
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { weddingId });
        }
    }
}
