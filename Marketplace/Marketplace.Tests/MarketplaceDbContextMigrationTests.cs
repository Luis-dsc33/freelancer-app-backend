using Marketplace.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Marketplace.Tests;

public class MarketplaceDbContextMigrationTests
{
    [Fact]
    public void MarketplaceDbContext_SinCambiosPendientesRespectoAMigraciones()
    {
        var options = new DbContextOptionsBuilder<MarketplaceDbContext>()
            .UseNpgsql("Host=localhost;Database=test;Username=postgres;Password=postgres")
            .Options;

        using var db = new MarketplaceDbContext(options);
        var migrator = db.GetService<IMigrationsAssembly>();
        var differ = db.GetService<IMigrationsModelDiffer>();
        var designTimeModel = db.GetService<Microsoft.EntityFrameworkCore.Metadata.IDesignTimeModel>();
        var snapshotModel = migrator.ModelSnapshot!.Model;
        var modelInitializer = db.GetService<Microsoft.EntityFrameworkCore.Infrastructure.IModelRuntimeInitializer>();
        var initializedSnapshot = modelInitializer.Initialize(snapshotModel);
        var diffs = differ.GetDifferences(
            initializedSnapshot.GetRelationalModel(),
            designTimeModel.Model.GetRelationalModel());

        var schemaDiffs = diffs
            .Where(d => d is not Microsoft.EntityFrameworkCore.Migrations.Operations.UpdateDataOperation)
            .ToList();

        Assert.True(schemaDiffs.Count == 0, string.Join(" | ", schemaDiffs.Select(d => d.GetType().Name)));
    }
}
