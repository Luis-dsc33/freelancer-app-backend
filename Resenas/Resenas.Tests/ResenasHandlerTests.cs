using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Resenas.Application.Commands.CalificarCliente;
using Resenas.Application.Exceptions;
using Resenas.Application.Interfaces;
using Resenas.Application.Queries.ObtenerEstadisticasResenas;
using Resenas.Application.Queries.ObtenerReputacionUsuario;
using Resenas.Domain.Entities;
using Resenas.Infrastructure.Persistence;

namespace Resenas.Tests;

public class ResenasHandlerTests
{
    [Fact]
    public void Validator_PuntuacionFueraDeRango_DevuelveError()
    {
        var validator = new CalificarClienteCommandValidator();
        var result = validator.TestValidate(new CalificarClienteCommand(Guid.NewGuid(), 6, "Muy bien"));
        result.ShouldHaveValidationErrorFor(x => x.Puntuacion);
    }

    [Fact]
    public async Task CalificarCliente_ContratoFinalizado_UsaAutorIdDelJwtYGuardaResena()
    {
        var estudianteId = Guid.NewGuid();
        var clienteId = Guid.NewGuid();
        var contratoId = Guid.NewGuid();

        var repo = new Mock<IResenaRepository>();
        var marketplace = new Mock<IMarketplaceContratosClient>();
        marketplace.Setup(x => x.ObtenerContratoAsync(contratoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ContratoResumenDto(contratoId, Guid.NewGuid(), Guid.NewGuid(), clienteId, estudianteId, "Finalizado", "Proyecto", "Cliente"));
        repo.Setup(x => x.ExisteAsync(contratoId, estudianteId, clienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        Resena? guardada = null;
        repo.Setup(x => x.AgregarAsync(It.IsAny<Resena>(), It.IsAny<CancellationToken>()))
            .Callback<Resena, CancellationToken>((r, _) => guardada = r)
            .Returns(Task.CompletedTask);

        var handler = new CalificarClienteHandler(repo.Object, marketplace.Object, UsuarioEstudiante(estudianteId));

        var dto = await handler.Handle(new CalificarClienteCommand(contratoId, 5, "Excelente comunicación"), CancellationToken.None);

        Assert.NotNull(guardada);
        Assert.Equal(estudianteId, guardada!.AutorId);
        Assert.Equal(clienteId, guardada.DestinatarioId);
        Assert.Equal(5, dto.Puntuacion);
    }

    [Fact]
    public async Task CalificarCliente_ContratoDeOtroEstudiante_LanzaUnauthorizedAccessException()
    {
        var estudianteId = Guid.NewGuid();
        var contratoId = Guid.NewGuid();
        var repo = new Mock<IResenaRepository>();
        var marketplace = new Mock<IMarketplaceContratosClient>();
        marketplace.Setup(x => x.ObtenerContratoAsync(contratoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ContratoResumenDto(contratoId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Finalizado", null, null));

        var handler = new CalificarClienteHandler(repo.Object, marketplace.Object, UsuarioEstudiante(estudianteId));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            handler.Handle(new CalificarClienteCommand(contratoId, 4, "Bueno"), CancellationToken.None));
    }

    [Fact]
    public async Task CalificarCliente_ContratoActivo_LanzaInvalidOperationException()
    {
        var estudianteId = Guid.NewGuid();
        var contratoId = Guid.NewGuid();
        var repo = new Mock<IResenaRepository>();
        var marketplace = new Mock<IMarketplaceContratosClient>();
        marketplace.Setup(x => x.ObtenerContratoAsync(contratoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ContratoResumenDto(contratoId, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), estudianteId, "Activo", null, null));

        var handler = new CalificarClienteHandler(repo.Object, marketplace.Object, UsuarioEstudiante(estudianteId));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new CalificarClienteCommand(contratoId, 4, "Aún activo"), CancellationToken.None));
    }

    [Fact]
    public async Task CalificarCliente_ResenaDuplicada_LanzaConflictException()
    {
        var estudianteId = Guid.NewGuid();
        var clienteId = Guid.NewGuid();
        var contratoId = Guid.NewGuid();
        var repo = new Mock<IResenaRepository>();
        var marketplace = new Mock<IMarketplaceContratosClient>();
        marketplace.Setup(x => x.ObtenerContratoAsync(contratoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ContratoResumenDto(contratoId, Guid.NewGuid(), Guid.NewGuid(), clienteId, estudianteId, "Finalizado", null, null));
        repo.Setup(x => x.ExisteAsync(contratoId, estudianteId, clienteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CalificarClienteHandler(repo.Object, marketplace.Object, UsuarioEstudiante(estudianteId));

        await Assert.ThrowsAsync<ConflictException>(() =>
            handler.Handle(new CalificarClienteCommand(contratoId, 5, "Duplicado"), CancellationToken.None));
    }

    [Fact]
    public async Task ObtenerReputacionUsuario_CalculaPromedioYTotalCorrectamente()
    {
        var usuarioId = Guid.NewGuid();
        var autor1 = Guid.NewGuid();
        var autor2 = Guid.NewGuid();

        var repo = new Mock<IResenaRepository>();
        repo.Setup(x => x.ListarPorDestinatarioAsync(usuarioId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new Resena { Id = Guid.NewGuid(), ContratoId = Guid.NewGuid(), AutorId = autor1, DestinatarioId = usuarioId, Puntuacion = 5, Comentario = "Excelente", CreadaEn = DateTime.UtcNow },
                new Resena { Id = Guid.NewGuid(), ContratoId = Guid.NewGuid(), AutorId = autor2, DestinatarioId = usuarioId, Puntuacion = 4, Comentario = "Muy bien", CreadaEn = DateTime.UtcNow.AddMinutes(-5) }
            ]);

        var usuarios = new Mock<IUsuariosPublicoClient>();
        usuarios.Setup(x => x.ObtenerNombresPorIdsAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<Guid, string> { [autor1] = "Carlos" });

        var handler = new ObtenerReputacionUsuarioHandler(repo.Object, usuarios.Object);
        var reputacion = await handler.Handle(new ObtenerReputacionUsuarioQuery(usuarioId), CancellationToken.None);

        Assert.Equal(4.5, reputacion.Promedio);
        Assert.Equal(2, reputacion.TotalResenas);
        Assert.Equal("Carlos", reputacion.Resenas[0].NombreAutor);
        Assert.Null(reputacion.Resenas[1].NombreAutor);
    }

    [Fact]
    public async Task ObtenerEstadisticasResenas_CalculaTotalPromedioYDistribucion1A5()
    {
        var repo = new Mock<IResenaRepository>();
        repo.Setup(x => x.ContarPorPuntuacionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<int, int>
            {
                [5] = 3,
                [4] = 1
            });

        var handler = new ObtenerEstadisticasResenasHandler(repo.Object);
        var stats = await handler.Handle(new ObtenerEstadisticasResenasQuery(), CancellationToken.None);

        Assert.Equal(4, stats.TotalCalificaciones);
        Assert.Equal(4.75, stats.PromedioGeneral);
        Assert.Equal(5, stats.DistribucionEstrellas.Count);
        Assert.Equal(0, stats.DistribucionEstrellas[0].Total);
        Assert.Equal(1, stats.DistribucionEstrellas[3].Total);
        Assert.Equal(3, stats.DistribucionEstrellas[4].Total);
    }

    [Fact]
    public void ResenasDbContext_SinCambiosPendientesRespectoAMigracion()
    {
        var options = new DbContextOptionsBuilder<ResenasDbContext>()
            .UseNpgsql("Host=localhost;Database=test;Username=postgres;Password=postgres")
            .Options;
        using var db = new ResenasDbContext(options);

        Assert.False(db.Database.HasPendingModelChanges());
    }

    private static ICurrentUserService UsuarioEstudiante(Guid id)
    {
        var mock = new Mock<ICurrentUserService>();
        mock.SetupGet(x => x.IsAuthenticated).Returns(true);
        mock.SetupGet(x => x.UserId).Returns(id);
        mock.SetupGet(x => x.Role).Returns("Estudiante");
        mock.SetupGet(x => x.EsEstudiante).Returns(true);
        return mock.Object;
    }
}
