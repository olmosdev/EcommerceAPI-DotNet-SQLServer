
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    // This represents a table in the database.
    public DbSet<Category> Categories { get; set; }
}
