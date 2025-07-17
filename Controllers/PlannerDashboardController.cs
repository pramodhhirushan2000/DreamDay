using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using DreamDay.Models;

namespace DreamDay.Controllers
{
    [Authorize(Roles = "Planner")]
    public class PlannerDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PlannerDashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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

            var assignedWeddings = await _context.Weddings
                .Where(w => w.PlannerId == user.Id)
                .ToListAsync();

            return View(assignedWeddings);
        }

        // GET: PlannerDashboard/WeddingDetails/5
        public async Task<IActionResult> WeddingDetails(int id)
        {
            var wedding = await _context.Weddings.FirstOrDefaultAsync(w => w.WeddingId == id);

            if (wedding == null)
            {
                return NotFound();
            }

            var checklistItems = await _context.ChecklistItems
                .Where(c => c.WeddingId == wedding.WeddingId)
                .ToListAsync();

            var guests = await _context.Guests
                .Where(g => g.WeddingId == wedding.WeddingId)
                .ToListAsync();

            var budgetItems = await _context.BudgetItems
                .Where(b => b.WeddingId == wedding.WeddingId)
                .ToListAsync();

            var model = new WeddingDetailsViewModel
            {
                Wedding = wedding,
                ChecklistItems = checklistItems,
                Guests = guests,
                BudgetItems = budgetItems
            };

            var assignedVendors = await _context.VendorAssignments
                .Include(va => va.Vendor)
                .Where(va => va.WeddingId == id)
                .ToListAsync();

            ViewBag.AssignedVendors = assignedVendors;

            return View(model);
        }

        // POST: PlannerDashboard/SendMessage
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageInputModel input)
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

        // GET: PlannerDashboard/GetMessages
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

        // Internal class for receiving message input
        public class MessageInputModel
        {
            public int WeddingId { get; set; }
            public string Content { get; set; }
        }

    }
}
