using Microsoft.EntityFrameworkCore;
using Session19_Api.Models;

namespace Session19_Api.Data;

// The DbContext is EF Core's bridge to the database.
// Each DbSet<T> becomes a table you can query and save to.
// The options (which database, which connection string) are supplied
// by dependency injection in Program.cs — see AddDbContext there.
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}
