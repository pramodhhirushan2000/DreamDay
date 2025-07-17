using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models;
using System.Threading.Tasks;
using System.Linq;

namespace DreamDay.Controllers
{
    public class GuestManagementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public GuestManagementController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var wedding = await _context.Weddings.FirstOrDefaultAsync(w => w.CoupleId == user.Id);
            if (wedding == null) return RedirectToAction("Index", "Home");

            var guests = await _context.Guests
                .Where(g => g.WeddingId == wedding.WeddingId)
                .Include(g => g.Table)
                .ToListAsync();

            var tables = await _context.WeddingTables
                .Where(t => t.WeddingId == wedding.WeddingId)
                .ToListAsync();

            ViewBag.Tables = tables;
            ViewBag.WeddingId = wedding.WeddingId;
            return View(guests);
        }

        // GET: GuestManagement/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var wedding = await _context.Weddings.FirstOrDefaultAsync(w => w.CoupleId == user.Id);
            if (wedding == null) return RedirectToAction("Index");

            ViewBag.WeddingId = wedding.WeddingId;
            ViewBag.Tables = await _context.WeddingTables
                .Where(t => t.WeddingId == wedding.WeddingId)
                .ToListAsync();

            return View(new Guest { WeddingId = wedding.WeddingId });
        }

        // POST: GuestManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guest guest)
        {
            Console.WriteLine("POST Create() hit");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is INVALID");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
                    }
                }
                ViewBag.Tables = await _context.WeddingTables
                    .Where(t => t.WeddingId == guest.WeddingId)
                    .ToListAsync();

                return View(guest);
            }

            Console.WriteLine("ModelState is VALID — saving...");
            _context.Guests.Add(guest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: GuestManagement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests.FindAsync(id);

            if (guest == null)
            {
                return NotFound();
            }

            ViewBag.Tables = await _context.WeddingTables
                .Where(t => t.WeddingId == guest.WeddingId)
                .ToListAsync();

            return View(guest);
        }

        // POST: GuestManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Guest guest)
        {
            if (id != guest.GuestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Guests.Any(e => e.GuestId == guest.GuestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(guest);
        }

        // GET: GuestManagement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guest = await _context.Guests
                .FirstOrDefaultAsync(m => m.GuestId == id);

            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        // POST: GuestManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignTable(int guestId, int? tableId)
        {
            var guest = await _context.Guests.FindAsync(guestId);
            if (guest == null) return NotFound();

            if (tableId == null)
            {
                guest.TableId = null;
                _context.Update(guest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var table = await _context.WeddingTables.FindAsync(tableId);
            if (table == null) return NotFound();

            var currentSeats = await _context.Guests
                .Where(g => g.TableId == table.TableId && g.GuestId != guest.GuestId)
                .SumAsync(g => (int?)g.SeatNumber) ?? 0;

            int availableSeats = table.MaxSeats - currentSeats;

            if (guest.SeatNumber > availableSeats)
            {
                TempData["ErrorMessage"] = $"Not enough seats available at {table.TableName}. Only {availableSeats} seat(s) left.";
                return RedirectToAction(nameof(Index));
            }

            guest.TableId = tableId;
            _context.Update(guest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateTables(int weddingId, int seatsPerTable, int tableCount)
        {
            for (int i = 1; i <= tableCount; i++)
            {
                var table = new WeddingTable
                {
                    WeddingId = weddingId,
                    TableName = $"Table {i}",
                    MaxSeats = seatsPerTable
                };
                _context.WeddingTables.Add(table);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<JsonResult> GetAvailableSeats(int tableId)
        {
            var table = await _context.WeddingTables.FindAsync(tableId);
            if (table == null) return Json(new List<int>());

            var takenSeats = await _context.Guests
                .Where(g => g.TableId == tableId)
                .Select(g => g.SeatNumber)
                .ToListAsync();

            var allSeats = Enumerable.Range(1, table.MaxSeats);
            var availableSeats = allSeats.Except(takenSeats);

            return Json(availableSeats);
        }

    }
}
