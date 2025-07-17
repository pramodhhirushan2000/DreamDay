using Microsoft.AspNetCore.Mvc;
using DreamDay.Data;
using DreamDay.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Controllers
{
    public class VendorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string category, int page = 1)
        {
            int pageSize = 6;

            var vendorsQuery = _context.Vendors.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                vendorsQuery = vendorsQuery.Where(v => v.Category == category);
            }

            var totalVendors = await vendorsQuery.CountAsync();
            var vendors = await vendorsQuery
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

            ViewBag.TotalVendors = totalVendors;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.Categories = await _context.Vendors.Select(v => v.Category).Distinct().ToListAsync();
            ViewBag.SelectedCategory = category;

            return View(vendors);

        }
    }
}
