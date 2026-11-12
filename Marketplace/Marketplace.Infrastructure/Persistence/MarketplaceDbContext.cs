using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Persistence;

public class MarketplaceDbContext(DbContextOptions<MarketplaceDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("marketplace");
        base.OnModelCreating(modelBuilder);
    }
}
