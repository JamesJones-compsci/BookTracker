namespace BookTracker.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// ApplicationDbContext extends IdentityContext<User>
// This allows Identity to use our custom User class
public class ApplicationDbContext : IdentityDbContext<User>
{
    // Constructor receives DbContextOptions from Program.cs
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }
    
    
    // DbSets correspond to tables in PostgreSQL
    public DbSet<Book> Books { get; set; }
    public DbSet<Review> Reviews { get; set; }
}