using Microsoft.EntityFrameworkCore;

namespace Admin.Infrastructure.Persistence;

public class AdminDbContext(DbContextOptions<AdminDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("admin");
        base.OnModelCreating(modelBuilder);
    }
}
