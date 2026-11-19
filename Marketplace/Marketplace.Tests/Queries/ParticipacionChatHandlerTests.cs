using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.ParticipacionChat;
using Marketplace.Domain.Entities;
using Moq;

namespace Marketplace.Tests.Queries;

public class ParticipacionChatHandlerTests
{
    [Fact]
    public async Task EstudianteConPostulacionPendiente_QuedaAutorizado()
    {
        var estudianteId = Guid.NewGuid();
        var clienteId = Guid.NewGuid();
        var proyecto = new Proyecto { Id = Guid.NewGuid(), ClienteId = clienteId, Titulo = "App", NombreCliente = "Cliente" };
        var postulacion = new Postulacion
        {
            Id = Guid.NewGuid(),
            ProyectoId = proyecto.Id,
            EstudianteId = estudianteId,
            Estado = Postulacion.EstadoPendiente
        };

        var proyectos = new Mock<IProyectoRepository>();
        var postulaciones = new Mock<IPostulacionRepository>();
        var contratos = new Mock<IContratoRepository>();

        proyectos.Setup(x => x.ObtenerPorIdAsync(proyecto.Id, It.IsAny<CancellationToken>())).ReturnsAsync(proyecto);
        postulaciones.Setup(x => x.ObtenerPorIdAsync(postulacion.Id, It.IsAny<CancellationToken>())).ReturnsAsync(postulacion);
        contratos.Setup(x => x.ObtenerParaEstudianteEnProyectoAsync(proyecto.Id, estudianteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Contrato?)null);

        var handler = new ObtenerParticipacionChatHandler(
            proyectos.Object,
            postulaciones.Object,
            contratos.Object,
            Usuario(estudianteId, "Estudiante"));

        var res = await handler.Handle(new ObtenerParticipacionChatQuery(proyecto.Id, postulacion.Id, null), CancellationToken.None);

        Assert.True(res.Autorizado);
        Assert.Equal(clienteId, res.ClienteId);
        Assert.Equal(estudianteId, res.EstudianteId);
        Assert.Equal(postulacion.Id, res.PostulacionId);
    }

    [Fact]
    public async Task EstudianteConPostulacionRechazada_NoQuedaAutorizado()
    {
        var estudianteId = Guid.NewGuid();
        var proyecto = new Proyecto { Id = Guid.NewGuid(), ClienteId = Guid.NewGuid(), Titulo = "App", NombreCliente = "Cliente" };
        var postulacion = new Postulacion
        {
            Id = Guid.NewGuid(),
            ProyectoId = proyecto.Id,
            EstudianteId = estudianteId,
            Estado = Postulacion.EstadoRechazada
        };

        var proyectos = new Mock<IProyectoRepository>();
        var postulaciones = new Mock<IPostulacionRepository>();
        var contratos = new Mock<IContratoRepository>();

        proyectos.Setup(x => x.ObtenerPorIdAsync(proyecto.Id, It.IsAny<CancellationToken>())).ReturnsAsync(proyecto);
        postulaciones.Setup(x => x.ObtenerPorIdAsync(postulacion.Id, It.IsAny<CancellationToken>())).ReturnsAsync(postulacion);

        var handler = new ObtenerParticipacionChatHandler(
            proyectos.Object,
            postulaciones.Object,
            contratos.Object,
            Usuario(estudianteId, "Estudiante"));

        var res = await handler.Handle(new ObtenerParticipacionChatQuery(proyecto.Id, postulacion.Id, null), CancellationToken.None);

        Assert.False(res.Autorizado);
    }

    [Fact]
    public async Task EstudianteConContratoCancelado_NoQuedaAutorizado()
    {
        var estudianteId = Guid.NewGuid();
        var proyecto = new Proyecto { Id = Guid.NewGuid(), ClienteId = Guid.NewGuid(), Titulo = "App", NombreCliente = "Cliente" };
        var contrato = new Contrato
        {
            Id = Guid.NewGuid(),
            ProyectoId = proyecto.Id,
            EstudianteId = estudianteId,
            Estado = Contrato.EstadoCancelado
        };

        var proyectos = new Mock<IProyectoRepository>();
        var postulaciones = new Mock<IPostulacionRepository>();
        var contratos = new Mock<IContratoRepository>();

        proyectos.Setup(x => x.ObtenerPorIdAsync(proyecto.Id, It.IsAny<CancellationToken>())).ReturnsAsync(proyecto);
        contratos.Setup(x => x.ObtenerPorIdAsync(contrato.Id, It.IsAny<CancellationToken>())).ReturnsAsync(contrato);

        var handler = new ObtenerParticipacionChatHandler(
            proyectos.Object,
            postulaciones.Object,
            contratos.Object,
            Usuario(estudianteId, "Estudiante"));

        var res = await handler.Handle(new ObtenerParticipacionChatQuery(proyecto.Id, null, contrato.Id), CancellationToken.None);

        Assert.False(res.Autorizado);
    }

    private static ICurrentUserService Usuario(Guid id, string role)
    {
        var mock = new Mock<ICurrentUserService>();
        mock.SetupGet(x => x.IsAuthenticated).Returns(true);
        mock.SetupGet(x => x.UserId).Returns(id);
        mock.SetupGet(x => x.Role).Returns(role);
        mock.SetupGet(x => x.EsEstudiante).Returns(role == "Estudiante");
        mock.SetupGet(x => x.EsCliente).Returns(role == "Cliente");
        return mock.Object;
    }
}
