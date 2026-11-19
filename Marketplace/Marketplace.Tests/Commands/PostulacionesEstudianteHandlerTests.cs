using FluentValidation.TestHelper;
using Marketplace.Application.Commands.PostularAOferta;
using Marketplace.Application.Commands.ResolverPostulacion;
using Marketplace.Application.Commands.RetirarPostulacion;
using Marketplace.Application.Exceptions;
using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Enums;
using Moq;

namespace Marketplace.Tests.Commands;

public class PostulacionesEstudianteHandlerTests
{
    [Fact]
    public void Validator_MensajeCorto_DevuelveError()
    {
        var validator = new PostularAOfertaCommandValidator();
        var result = validator.TestValidate(new PostularAOfertaCommand(Guid.NewGuid(), "Hola"));
        result.ShouldHaveValidationErrorFor(x => x.Mensaje);
    }

    [Fact]
    public async Task PostularAOferta_OfertaPublicadaYValida_UsaEstudianteIdDelJwtYCreaPendiente()
    {
        var estudianteId = Guid.NewGuid();
        var proyecto = CrearProyecto(Proyecto.EstadoPublicada);
        var proyectos = new Mock<IProyectoRepository>();
        var postulaciones = new Mock<IPostulacionRepository>();
        var currentUser = UsuarioEstudiante(estudianteId);

        proyectos.Setup(x => x.ObtenerPorIdAsync(proyecto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(proyecto);
        postulaciones.Setup(x => x.ExisteParaEstudianteAsync(proyecto.Id, estudianteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        Postulacion? agregada = null;
        postulaciones.Setup(x => x.AgregarAsync(It.IsAny<Postulacion>(), It.IsAny<CancellationToken>()))
            .Callback<Postulacion, CancellationToken>((p, _) => agregada = p)
            .Returns(Task.CompletedTask);

        var handler = new PostularAOfertaHandler(proyectos.Object, postulaciones.Object, currentUser);

        var dto = await handler.Handle(
            new PostularAOfertaCommand(proyecto.Id, "Tengo experiencia desarrollando soluciones en Angular y .NET."),
            CancellationToken.None);

        Assert.NotNull(agregada);
        Assert.Equal(estudianteId, agregada!.EstudianteId);
        Assert.Equal(Postulacion.EstadoPendiente, dto.Estado);
        Assert.Equal(proyecto.Titulo, dto.TituloProyecto);
    }

    [Fact]
    public async Task PostularAOferta_Duplicada_LanzaConflictException()
    {
        var estudianteId = Guid.NewGuid();
        var proyecto = CrearProyecto(Proyecto.EstadoAbierto);
        var proyectos = new Mock<IProyectoRepository>();
        var postulaciones = new Mock<IPostulacionRepository>();
        proyectos.Setup(x => x.ObtenerPorIdAsync(proyecto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(proyecto);
        postulaciones.Setup(x => x.ExisteParaEstudianteAsync(proyecto.Id, estudianteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new PostularAOfertaHandler(proyectos.Object, postulaciones.Object, UsuarioEstudiante(estudianteId));

        await Assert.ThrowsAsync<ConflictException>(() =>
            handler.Handle(new PostularAOfertaCommand(proyecto.Id, "Propuesta suficientemente larga para validar."), CancellationToken.None));
    }

    [Fact]
    public async Task PostularAOferta_PerfilIncompletoEnUsuarios_LanzaInvalidOperationException()
    {
        var estudianteId = Guid.NewGuid();
        var proyecto = CrearProyecto(Proyecto.EstadoAbierto);
        var proyectos = new Mock<IProyectoRepository>();
        var postulaciones = new Mock<IPostulacionRepository>();
        var usuariosClient = new Mock<IUsuariosEstudianteClient>();

        proyectos.Setup(x => x.ObtenerPorIdAsync(proyecto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(proyecto);
        postulaciones.Setup(x => x.ExisteParaEstudianteAsync(proyecto.Id, estudianteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        usuariosClient.Setup(x => x.ObtenerEstadoEstudianteAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstadoElegibilidadEstudiante(PerfilCompleto: false, TienePortafolio: false));

        var handler = new PostularAOfertaHandler(
            proyectos.Object,
            postulaciones.Object,
            UsuarioEstudiante(estudianteId),
            usuariosClient.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new PostularAOfertaCommand(proyecto.Id, "Propuesta suficientemente larga para validar."), CancellationToken.None));

        postulaciones.Verify(x => x.AgregarAsync(It.IsAny<Postulacion>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task RetirarPostulacion_OtroEstudiante_LanzaUnauthorizedAccessException()
    {
        var duenoId = Guid.NewGuid();
        var otroId = Guid.NewGuid();
        var postulacion = new Postulacion
        {
            Id = Guid.NewGuid(),
            ProyectoId = Guid.NewGuid(),
            EstudianteId = duenoId,
            Estado = Postulacion.EstadoPendiente,
            Mensaje = "Propuesta",
            CreadaEn = DateTime.UtcNow
        };
        var postulaciones = new Mock<IPostulacionRepository>();
        postulaciones.Setup(x => x.ObtenerPorIdAsync(postulacion.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(postulacion);

        var handler = new RetirarPostulacionHandler(postulaciones.Object, UsuarioEstudiante(otroId));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            handler.Handle(new RetirarPostulacionCommand(postulacion.Id), CancellationToken.None));
    }

    [Fact]
    public async Task RetirarPostulacion_YaAceptada_LanzaInvalidOperationException()
    {
        var estudianteId = Guid.NewGuid();
        var postulacion = new Postulacion
        {
            Id = Guid.NewGuid(),
            ProyectoId = Guid.NewGuid(),
            EstudianteId = estudianteId,
            Estado = Postulacion.EstadoAceptada,
            Mensaje = "Propuesta",
            CreadaEn = DateTime.UtcNow
        };
        var postulaciones = new Mock<IPostulacionRepository>();
        postulaciones.Setup(x => x.ObtenerPorIdAsync(postulacion.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(postulacion);

        var handler = new RetirarPostulacionHandler(postulaciones.Object, UsuarioEstudiante(estudianteId));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new RetirarPostulacionCommand(postulacion.Id), CancellationToken.None));
    }

    [Fact]
    public async Task ResolverPostulacion_OfertaEnEstadoPublicada_PermiteAceptar()
    {
        var clienteId = Guid.NewGuid();
        var proyecto = CrearProyecto(Proyecto.EstadoPublicada, clienteId);
        var postulacion = new Postulacion
        {
            Id = Guid.NewGuid(),
            ProyectoId = proyecto.Id,
            EstudianteId = Guid.NewGuid(),
            Estado = Postulacion.EstadoPendiente,
            Mensaje = "Propuesta",
            CreadaEn = DateTime.UtcNow,
            Proyecto = proyecto
        };

        var postulaciones = new Mock<IPostulacionRepository>();
        postulaciones.Setup(x => x.ObtenerPorIdAsync(postulacion.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(postulacion);
        postulaciones.Setup(x => x.ObtenerPorProyectoAsync(proyecto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync([postulacion]);

        var currentUser = new Mock<ICurrentUserService>();
        currentUser.SetupGet(x => x.IsAuthenticated).Returns(true);
        currentUser.SetupGet(x => x.UserId).Returns(clienteId);
        currentUser.SetupGet(x => x.Role).Returns("Cliente");
        currentUser.SetupGet(x => x.EsCliente).Returns(true);

        var handler = new ResolverPostulacionHandler(postulaciones.Object, currentUser.Object);

        var dto = await handler.Handle(
            new ResolverPostulacionCommand(postulacion.Id, Postulacion.EstadoAceptada),
            CancellationToken.None);

        Assert.Equal(Postulacion.EstadoAceptada, dto.Estado);
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

    private static Proyecto CrearProyecto(string estado, Guid? clienteId = null) => new()
    {
        Id = Guid.NewGuid(),
        Titulo = "App Campus",
        Descripcion = "Desarrollo de aplicación móvil",
        Disciplina = "Desarrollo de software",
        Presupuesto = 8000m,
        FechaLimite = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(5),
        ClienteId = clienteId ?? Guid.NewGuid(),
        NombreCliente = "CodeBridge Lab",
        Modalidad = ModalidadProyecto.Remoto,
        Estado = estado,
        CreadoEn = DateTime.UtcNow
    };
}
