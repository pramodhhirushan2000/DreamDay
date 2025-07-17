using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;

namespace DreamDay.Controllers
{
    [Authorize(Roles = "Planner")]
    public class PlannerVendorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlannerVendorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int weddingId)
        {
            var vendors = await _context.Vendors.ToListAsync();
            ViewBag.WeddingId = weddingId;
            return View(vendors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignVendor(int vendorId, int weddingId)
        {
            var vendor = await _context.Vendors.FindAsync(vendorId);
            var wedding = await _context.Weddings.FindAsync(weddingId);

            if (vendor == null || wedding == null)
            {
                return NotFound();
            }

            // Check if already assigned
            bool alreadyAssigned = await _context.VendorAssignments
                .AnyAsync(va => va.VendorId == vendorId && va.WeddingId == weddingId);

            if (alreadyAssigned)
            {
                TempData["ErrorMessage"] = "This vendor is already assigned to this wedding.";
                return RedirectToAction(nameof(Index), new { weddingId });
            }

            // Check vendor availability
            if (vendor.AvailableFrom.HasValue && vendor.AvailableTo.HasValue)
            {
                if (wedding.WeddingDate < vendor.AvailableFrom || wedding.WeddingDate > vendor.AvailableTo)
                {
                    TempData["ErrorMessage"] = "Vendor is not available on the wedding date.";
                    return RedirectToAction(nameof(Index), new { weddingId });
                }
            }

            var assignment = new VendorAssignment
            {
                VendorId = vendorId,
                WeddingId = weddingId
            };

            _context.VendorAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Vendor assigned successfully!";
            return RedirectToAction(nameof(Index), new { weddingId });
        }

    }
}
