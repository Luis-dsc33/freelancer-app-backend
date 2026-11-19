using Marketplace.Application.Commands.CambiarEstadoContrato;
using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.Contratos;
using Marketplace.Application.Queries.ObtenerDetalleOportunidad;
using Marketplace.Domain.Entities;
using Moq;

namespace Marketplace.Tests.Commands;

public class ContratosYDetalleEstudianteHandlerTests
{
    [Fact]
    public async Task CambiarEstadoContrato_EstudianteParticipanteFinalizaActivo_ActualizaEstadoYCerradoEn()
    {
        var estudianteId = Guid.NewGuid();
        var contrato = new Contrato
        {
            Id = Guid.NewGuid(),
            PostulacionId = Guid.NewGuid(),
            ProyectoId = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            EstudianteId = estudianteId,
            ImporteAcordado = 5000m,
            FechaInicio = DateOnly.FromDateTime(DateTime.UtcNow),
            FechaCompromiso = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(14),
            Estado = Contrato.EstadoActivo,
            CreadoEn = DateTime.UtcNow
        };

        var repo = new Mock<IContratoRepository>();
        repo.Setup(x => x.ObtenerPorIdAsync(contrato.Id, It.IsAny<CancellationToken>())).ReturnsAsync(contrato);

        var currentUser = UsuarioEstudiante(estudianteId);
        var handler = new CambiarEstadoContratoHandler(repo.Object, currentUser);

        var dto = await handler.Handle(
            new CambiarEstadoContratoCommand(contrato.Id, Contrato.EstadoFinalizado),
            CancellationToken.None);

        Assert.Equal(Contrato.EstadoFinalizado, dto.Estado);
        Assert.NotNull(dto.CerradoEn);
        repo.Verify(x => x.ActualizarAsync(contrato, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CambiarEstadoContrato_ContratoCancelado_LanzaInvalidOperationException()
    {
        var estudianteId = Guid.NewGuid();
        var contrato = new Contrato
        {
            Id = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            EstudianteId = estudianteId,
            Estado = Contrato.EstadoCancelado
        };
        var repo = new Mock<IContratoRepository>();
        repo.Setup(x => x.ObtenerPorIdAsync(contrato.Id, It.IsAny<CancellationToken>())).ReturnsAsync(contrato);

        var handler = new CambiarEstadoContratoHandler(repo.Object, UsuarioEstudiante(estudianteId));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new CambiarEstadoContratoCommand(contrato.Id, Contrato.EstadoFinalizado), CancellationToken.None));
    }

    [Fact]
    public async Task ObtenerContratoPorId_UsuarioAjeno_LanzaUnauthorizedAccessException()
    {
        var contrato = new Contrato
        {
            Id = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(),
            EstudianteId = Guid.NewGuid(),
            Estado = Contrato.EstadoActivo
        };
        var repo = new Mock<IContratoRepository>();
        repo.Setup(x => x.ObtenerPorIdAsync(contrato.Id, It.IsAny<CancellationToken>())).ReturnsAsync(contrato);

        var handler = new ObtenerContratoPorIdHandler(repo.Object, UsuarioEstudiante(Guid.NewGuid()));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            handler.Handle(new ObtenerContratoPorIdQuery(contrato.Id), CancellationToken.None));
    }

    [Fact]
    public async Task ObtenerDetalleOportunidad_OcultaSinRelacionParaEstudiante_LanzaKeyNotFoundException()
    {
        var estudianteId = Guid.NewGuid();
        var proyecto = new Proyecto
        {
            Id = Guid.NewGuid(),
            Titulo = "Oferta oculta",
            Estado = Proyecto.EstadoOculta,
            FechaLimite = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(10)
        };

        var proyectos = new Mock<IProyectoRepository>();
        var postulaciones = new Mock<IPostulacionRepository>();
        var contratos = new Mock<IContratoRepository>();

        proyectos.Setup(x => x.ObtenerPorIdAsync(proyecto.Id, It.IsAny<CancellationToken>())).ReturnsAsync(proyecto);
        postulaciones.Setup(x => x.ExisteParaEstudianteAsync(proyecto.Id, estudianteId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        contratos.Setup(x => x.ObtenerParaEstudianteEnProyectoAsync(proyecto.Id, estudianteId, It.IsAny<CancellationToken>())).ReturnsAsync((Contrato?)null);

        var handler = new ObtenerDetalleOportunidadHandler(
            proyectos.Object,
            postulaciones.Object,
            contratos.Object,
            UsuarioEstudiante(estudianteId));

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(new ObtenerDetalleOportunidadQuery(proyecto.Id), CancellationToken.None));
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
