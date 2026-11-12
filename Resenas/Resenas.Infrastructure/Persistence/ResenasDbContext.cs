using Microsoft.EntityFrameworkCore;

namespace Resenas.Infrastructure.Persistence;

public class ResenasDbContext(DbContextOptions<ResenasDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("resenas");
        base.OnModelCreating(modelBuilder);
    }
}
