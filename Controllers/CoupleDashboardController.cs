using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using DreamDay.Data;
using DreamDay.Models;
using DreamDay.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace DreamDay.Controllers
{
    [Authorize(Roles = "Couple")]
    public class CoupleDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CoupleDashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var wedding = await _context.Weddings
                .FirstOrDefaultAsync(w => w.CoupleId == user.Id);

            if (wedding == null)
            {
                return View(new CoupleDashboardViewModel()); // Empty model
            }

            var totalTasks = await _context.ChecklistItems.CountAsync(c => c.WeddingId == wedding.WeddingId);
            var completedTasks = await _context.ChecklistItems.CountAsync(c => c.WeddingId == wedding.WeddingId && c.IsCompleted);
            var totalGuests = await _context.Guests.CountAsync(g => g.WeddingId == wedding.WeddingId);
            var acceptedGuests = await _context.Guests.CountAsync(g => g.WeddingId == wedding.WeddingId && g.RSVPStatus == "Accepted");
            var estimatedBudget = await _context.BudgetItems.Where(b => b.WeddingId == wedding.WeddingId).SumAsync(b => (decimal?)b.EstimatedCost) ?? 0;
            var actualBudget = await _context.BudgetItems.Where(b => b.WeddingId == wedding.WeddingId).SumAsync(b => (decimal?)b.ActualCost) ?? 0;

            var model = new CoupleDashboardViewModel
            {
                Wedding = wedding,
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                TotalGuests = totalGuests,
                AcceptedGuests = acceptedGuests,
                EstimatedBudget = estimatedBudget,
                ActualBudget = actualBudget
            };

            return View(model);
        }

        // POST: CoupleDashboard/SendMessage
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] PlannerDashboardController.MessageInputModel input)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Unauthorized();

            var wedding = await _context.Weddings.FirstOrDefaultAsync(w => w.WeddingId == input.WeddingId);

            if (wedding == null) return NotFound();

            var message = new Message
            {
                WeddingId = input.WeddingId,
                SenderId = user.Id,
                ReceiverId = (user.Id == wedding.CoupleId) ? wedding.PlannerId : wedding.CoupleId,
                Content = input.Content,
                SentDate = DateTime.Now
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET: CoupleDashboard/GetMessages
        public async Task<IActionResult> GetMessages(int weddingId)
        {
            var messages = await _context.Messages
                .Where(m => m.WeddingId == weddingId)
                .OrderBy(m => m.SentDate)
                .Select(m => new
                {
                    senderEmail = _context.Users.FirstOrDefault(u => u.Id == m.SenderId).Email,
                    content = m.Content,
                    sentTime = m.SentDate.ToString("g")
                })
                .ToListAsync();

            return Json(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOverview(int WeddingId, string Title, DateTime WeddingDate, string Venue)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var wedding = await _context.Weddings.FirstOrDefaultAsync(w => w.WeddingId == WeddingId && w.CoupleId == user.Id);
            if (wedding == null) return NotFound();

            wedding.Title = Title;
            wedding.WeddingDate = WeddingDate;
            wedding.Venue = Venue;

            _context.Weddings.Update(wedding);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Wedding details updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOverview(string Title, DateTime WeddingDate, string Venue)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var existingWedding = await _context.Weddings.FirstOrDefaultAsync(w => w.CoupleId == user.Id);
            if (existingWedding != null)
            {
                TempData["ErrorMessage"] = "Wedding overview already exists.";
                return RedirectToAction("Index");
            }

            var newWedding = new Wedding
            {
                CoupleId = user.Id,
                Title = Title,
                WeddingDate = WeddingDate,
                Venue = Venue
            };

            _context.Weddings.Add(newWedding);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Wedding overview created successfully.";
            return RedirectToAction("Index");
        }

    }
}
