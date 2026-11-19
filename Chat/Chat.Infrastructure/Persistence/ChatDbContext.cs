using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Persistence;

public class ChatDbContext(DbContextOptions<ChatDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("chat");
        base.OnModelCreating(modelBuilder);
    }
}
