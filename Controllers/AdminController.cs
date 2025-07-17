using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DreamDay.Data;
using DreamDay.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DreamDay.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Dashboard Home
        public async Task<IActionResult> Index()
        {
            var userCount = await _userManager.Users.CountAsync();
            var vendorCount = await _context.Vendors.CountAsync();
            var weddingCount = await _context.Weddings.CountAsync();

            ViewBag.UserCount = userCount;
            ViewBag.VendorCount = vendorCount;
            ViewBag.WeddingCount = weddingCount;

            return View();
        }

        // Manage Users
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // Manage Vendors
        public async Task<IActionResult> ManageVendors()
        {
            var vendors = await _context.Vendors.ToListAsync();
            return View(vendors);
        }

        // Delete Vendor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor != null)
            {
                _context.Vendors.Remove(vendor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageVendors));
        }

        public async Task<IActionResult> SystemReports()
        {
            var users = await _userManager.Users.ToListAsync();
            var vendors = await _context.Vendors.ToListAsync();
            var weddings = await _context.Weddings.ToListAsync();

            // Build role-based user statistics
            var couples = 0;
            var planners = 0;
            var admins = 0;

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Couple")) couples++;
                if (roles.Contains("Planner")) planners++;
                if (roles.Contains("Admin")) admins++;
            }

            // Vendor Category Summary
            var vendorSummary = vendors
                .GroupBy(v => v.Category)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToList();

            ViewBag.CoupleCount = couples;
            ViewBag.PlannerCount = planners;
            ViewBag.AdminCount = admins;
            ViewBag.VendorSummary = vendorSummary;
            ViewBag.WeddingCount = weddings.Count;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction(nameof(ManageUsers));

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction(nameof(ManageUsers));
        }

    }
}
