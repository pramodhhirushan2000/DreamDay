using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models;
using System.Threading.Tasks;
using System.Linq;

namespace DreamDay.Controllers
{
    public class WeddingTableController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WeddingTableController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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

            var tables = await _context.WeddingTables
                .Where(t => t.WeddingId == wedding.WeddingId)
                .ToListAsync();

            ViewBag.WeddingId = wedding.WeddingId;
            return View(tables);
        }

        public IActionResult Create(int weddingId)
        {
            ViewBag.WeddingId = weddingId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WeddingTable table)
        {
            if (ModelState.IsValid)
            {
                _context.Add(table);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(table);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var table = await _context.WeddingTables.FindAsync(id);
            if (table == null) return NotFound();

            return View(table);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WeddingTable table)
        {
            if (id != table.TableId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(table);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(table);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var table = await _context.WeddingTables.FindAsync(id);
            if (table == null) return NotFound();

            return View(table);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var table = await _context.WeddingTables.FindAsync(id);
            if (table != null)
            {
                _context.WeddingTables.Remove(table);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
