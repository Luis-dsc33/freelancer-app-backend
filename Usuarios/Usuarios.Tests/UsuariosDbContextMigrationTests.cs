using Microsoft.EntityFrameworkCore;
using Usuarios.Infrastructure.Persistence;

namespace Usuarios.Tests;

public class UsuariosDbContextMigrationTests
{
    [Fact]
    public void UsuariosDbContext_SinCambiosPendientesRespectoAMigraciones()
    {
        var options = new DbContextOptionsBuilder<UsuariosDbContext>()
            .UseNpgsql("Host=localhost;Database=test;Username=postgres;Password=postgres")
            .Options;

        using var db = new UsuariosDbContext(options);
        Assert.False(db.Database.HasPendingModelChanges());
    }
}
