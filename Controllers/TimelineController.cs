using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models;
using System.Threading.Tasks;
using System.Linq;

namespace DreamDay.Controllers
{
    public class TimelineController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TimelineController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Timeline/Index
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var wedding = await _context.Weddings.FirstOrDefaultAsync(w => w.CoupleId == user.Id);
            if (wedding == null) return RedirectToAction("Index", "Home");

            var events = await _context.TimelineEvents
                .Include(t => t.Vendor)
                .Where(e => e.WeddingId == wedding.WeddingId)
                .OrderBy(e => e.EventTime)
                .ToListAsync();

            ViewBag.WeddingId = wedding.WeddingId;
            return View(events);
        }

        // GET: Timeline/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            var wedding = await _context.Weddings.FirstOrDefaultAsync(w => w.CoupleId == user.Id);
            if (wedding == null) return RedirectToAction("Index", "Home");

            ViewBag.WeddingId = wedding.WeddingId;
            ViewBag.Vendors = await _context.Vendors.ToListAsync();

            return View(new TimelineEvent
            {
                WeddingId = wedding.WeddingId,
                EventTime = DateTime.Now
            });
        }

        // POST: Timeline/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimelineEvent timelineEvent)
        {
            Console.WriteLine(">>> CREATE POST HIT");
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"{state.Key} => {error.ErrorMessage}");
                    }
                }

                ViewBag.WeddingId = timelineEvent.WeddingId;
                ViewBag.Vendors = await _context.Vendors.ToListAsync();
                return View(timelineEvent);
            }

            _context.TimelineEvents.Add(timelineEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Timeline/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var timelineEvent = await _context.TimelineEvents.FindAsync(id);
            if (timelineEvent == null) return NotFound();

            ViewBag.WeddingId = timelineEvent.WeddingId;
            ViewBag.Vendors = await _context.Vendors.ToListAsync();
            return View(timelineEvent);
        }

        // POST: Timeline/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TimelineEvent timelineEvent)
        {
            if (id != timelineEvent.EventId) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.WeddingId = timelineEvent.WeddingId;
                ViewBag.Vendors = await _context.Vendors.ToListAsync();
                return View(timelineEvent);
            }

            _context.Update(timelineEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Timeline/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var timelineEvent = await _context.TimelineEvents
                .Include(t => t.Vendor)
                .FirstOrDefaultAsync(t => t.EventId == id);

            if (timelineEvent == null) return NotFound();

            return View(timelineEvent);
        }

        // POST: Timeline/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timelineEvent = await _context.TimelineEvents.FindAsync(id);
            if (timelineEvent != null)
            {
                _context.TimelineEvents.Remove(timelineEvent);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
