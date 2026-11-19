using Chat.Application.Commands.AbrirConversacion;
using Chat.Application.Commands.EnviarMensajeChat;
using Chat.Application.Dtos;
using Chat.Application.Interfaces;
using Chat.Application.Queries.ListarMensajesConversacion;
using Chat.Domain.Entities;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Chat.Tests;

public class ChatHandlerTests
{
    [Fact]
    public async Task AbrirConversacion_NoAutorizadoEnMarketplace_LanzaUnauthorizedAccessException()
    {
        var estudianteId = Guid.NewGuid();
        var proyectoId = Guid.NewGuid();
        var convRepo = new Mock<IConversacionRepository>();
        var msgRepo = new Mock<IMensajeRepository>();
        var marketplace = new Mock<IMarketplaceChatClient>();

        marketplace.Setup(x => x.ValidarParticipacionAsync(proyectoId, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ParticipacionChatDto(false, Guid.NewGuid(), Guid.Empty, "Proyecto", "Cliente", null, null));

        var handler = new AbrirConversacionHandler(convRepo.Object, msgRepo.Object, marketplace.Object, Usuario(estudianteId, "Estudiante"));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            handler.Handle(new AbrirConversacionCommand(proyectoId), CancellationToken.None));

        convRepo.Verify(x => x.AgregarAsync(It.IsAny<Conversacion>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task AbrirConversacion_PrimeraVez_CreaConversacionYMarcaEsNueva()
    {
        var estudianteId = Guid.NewGuid();
        var clienteId = Guid.NewGuid();
        var proyectoId = Guid.NewGuid();
        var convRepo = new Mock<IConversacionRepository>();
        var msgRepo = new Mock<IMensajeRepository>();
        var marketplace = new Mock<IMarketplaceChatClient>();

        marketplace.Setup(x => x.ValidarParticipacionAsync(proyectoId, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ParticipacionChatDto(true, clienteId, estudianteId, "Portal Web", "Empresa X", Guid.NewGuid(), null));
        convRepo.Setup(x => x.ObtenerPorParticipantesAsync(proyectoId, clienteId, estudianteId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Conversacion?)null);

        var handler = new AbrirConversacionHandler(convRepo.Object, msgRepo.Object, marketplace.Object, Usuario(estudianteId, "Estudiante"));

        var resultado = await handler.Handle(new AbrirConversacionCommand(proyectoId), CancellationToken.None);

        Assert.True(resultado.EsNueva);
        Assert.Equal("Portal Web", resultado.Conversacion.TituloProyecto);
        Assert.Equal("Empresa X", resultado.Conversacion.NombreContraparte);
        convRepo.Verify(x => x.AgregarAsync(It.IsAny<Conversacion>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ListarMensajes_UsuarioAjenoALaConversacion_LanzaUnauthorizedAccessException()
    {
        var conv = new Conversacion(Guid.NewGuid(), null, null, Guid.NewGuid(), Guid.NewGuid(), "Proyecto");
        var convRepo = new Mock<IConversacionRepository>();
        var msgRepo = new Mock<IMensajeRepository>();
        convRepo.Setup(x => x.ObtenerPorIdAsync(conv.Id, It.IsAny<CancellationToken>())).ReturnsAsync(conv);

        var handler = new ListarMensajesConversacionHandler(convRepo.Object, msgRepo.Object, Usuario(Guid.NewGuid(), "Estudiante"));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            handler.Handle(new ListarMensajesConversacionQuery(conv.Id), CancellationToken.None));
    }

    [Fact]
    public async Task EnviarMensajeChat_ParticipanteValido_GuardaUsaRemitenteJwtYNotificaSignalR()
    {
        var estudianteId = Guid.NewGuid();
        var conv = new Conversacion(Guid.NewGuid(), null, null, Guid.NewGuid(), estudianteId, "Proyecto");
        var convRepo = new Mock<IConversacionRepository>();
        var msgRepo = new Mock<IMensajeRepository>();
        var notifier = new Mock<IChatNotifier>();

        convRepo.Setup(x => x.ObtenerPorIdAsync(conv.Id, It.IsAny<CancellationToken>())).ReturnsAsync(conv);

        Mensaje? guardado = null;
        msgRepo.Setup(x => x.AgregarAsync(It.IsAny<Mensaje>(), It.IsAny<CancellationToken>()))
            .Callback<Mensaje, CancellationToken>((m, _) => guardado = m)
            .Returns(Task.CompletedTask);

        var handler = new EnviarMensajeChatHandler(convRepo.Object, msgRepo.Object, notifier.Object, Usuario(estudianteId, "Estudiante"));

        var dto = await handler.Handle(new EnviarMensajeChatCommand(conv.Id, "  Hola cliente  "), CancellationToken.None);

        Assert.NotNull(guardado);
        Assert.Equal(estudianteId, guardado!.RemitenteId);
        Assert.Equal("Hola cliente", dto.Contenido);
        notifier.Verify(x => x.NotificarMensajeAsync(conv.Id, It.Is<MensajeChatDto>(m => m.Id == dto.Id), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void ChatDbContext_SinCambiosPendientesRespectoAMigracion()
    {
        var options = new DbContextOptionsBuilder<ChatDbContext>()
            .UseNpgsql("Host=localhost;Database=test;Username=postgres;Password=postgres")
            .Options;
        using var db = new ChatDbContext(options);

        Assert.False(db.Database.HasPendingModelChanges());
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
