using DreamDay.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Wedding> Weddings { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<ChecklistItem> ChecklistItems { get; set; }
    public DbSet<BudgetItem> BudgetItems { get; set; }
    public DbSet<WeddingTable> WeddingTables { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<TimelineEvent> TimelineEvents { get; set; }
    public DbSet<VendorAssignment> VendorAssignments { get; set; }
    public DbSet<Message> Messages { get; set; }

}
