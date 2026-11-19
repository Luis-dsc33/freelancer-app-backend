using Marketplace.Application.Commands.PublicarOfertaExistente;
using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Enums;
using Moq;

namespace Marketplace.Tests.Commands;

public class PublicarOfertaExistenteTests
{
    [Fact]
    public async Task OfertaOcultaPorModeracion_LanzaInvalidOperationExceptionYNoActualiza()
    {
        var clienteId = Guid.NewGuid();
        var proyecto = CrearProyectoValido(clienteId, Proyecto.EstadoOculta);

        var repository = new Mock<IProyectoRepository>();
        repository
            .Setup(r => r.ObtenerPorIdAsync(proyecto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(proyecto);

        var handler = new PublicarOfertaExistenteHandler(repository.Object, UsuarioAutenticado(clienteId));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new PublicarOfertaExistenteCommand(proyecto.Id), CancellationToken.None));

        repository.Verify(
            r => r.ActualizarAsync(It.IsAny<Proyecto>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task OfertaBorradorValida_CambiaAEstadoPublicada()
    {
        var clienteId = Guid.NewGuid();
        var proyecto = CrearProyectoValido(clienteId, Proyecto.EstadoBorrador);

        var repository = new Mock<IProyectoRepository>();
        repository
            .Setup(r => r.ObtenerPorIdAsync(proyecto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(proyecto);

        var handler = new PublicarOfertaExistenteHandler(repository.Object, UsuarioAutenticado(clienteId));

        var resultado = await handler.Handle(new PublicarOfertaExistenteCommand(proyecto.Id), CancellationToken.None);

        Assert.Equal(Proyecto.EstadoPublicada, resultado.Estado);
        Assert.Equal(Proyecto.EstadoPublicada, proyecto.Estado);
        repository.Verify(r => r.ActualizarAsync(proyecto, It.IsAny<CancellationToken>()), Times.Once);
    }

    private static ICurrentUserService UsuarioAutenticado(Guid clienteId)
    {
        var currentUser = new Mock<ICurrentUserService>();
        currentUser.SetupGet(u => u.IsAuthenticated).Returns(true);
        currentUser.SetupGet(u => u.UserId).Returns(clienteId);
        return currentUser.Object;
    }

    private static Proyecto CrearProyectoValido(Guid clienteId, string estado) => new()
    {
        Id = Guid.NewGuid(),
        ClienteId = clienteId,
        Titulo = "Portal escolar",
        Descripcion = "Desarrollo de portal",
        Disciplina = "Desarrollo de software",
        Presupuesto = 5000m,
        FechaLimite = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(7),
        Estado = estado,
        CreadoEn = DateTime.UtcNow,
        Skills =
        [
            new ProyectoSkill
            {
                SkillId = Guid.NewGuid(),
                TipoRequisito = TipoRequisitoSkill.Requerida
            }
        ]
    };
}

