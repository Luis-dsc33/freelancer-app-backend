using Moq;
using Usuarios.Application.Commands.CrearOActualizarPerfil;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Tests.Commands;

public class CrearOActualizarPerfilHandlerTests
{
    [Fact]
    public async Task Handle_ObtieneElUsuarioDelContextoAutenticado()
    {
        var usuarioAutenticadoId = Guid.NewGuid();
        var repository = new Mock<IPerfilEstudianteRepository>();
        var currentUser = new Mock<ICurrentUserService>();
        currentUser.Setup(user => user.UsuarioId).Returns(usuarioAutenticadoId);
        repository.Setup(repo => repo.ObtenerPorUsuarioIdAsync(usuarioAutenticadoId))
            .ReturnsAsync((PerfilEstudiante?)null);
        PerfilEstudiante? perfilGuardado = null;
        repository.Setup(repo => repo.AgregarAsync(It.IsAny<PerfilEstudiante>()))
            .Callback<PerfilEstudiante>(perfil => perfilGuardado = perfil)
            .Returns(Task.CompletedTask);

        var handler = new CrearOActualizarPerfilHandler(repository.Object, currentUser.Object);
        var command = new CrearOActualizarPerfilCommand(
            "Ingeniería",
            ["C#", " C# ", "SQL"],
            "  Desarrollo de software  ");

        var perfilId = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(perfilGuardado);
        Assert.Equal(perfilId, perfilGuardado.Id);
        Assert.Equal(usuarioAutenticadoId, perfilGuardado.UsuarioId);
        Assert.Equal(["C#", "SQL"], perfilGuardado.Habilidades);
        Assert.Equal("Ingeniería", perfilGuardado.Carrera);
        Assert.Equal("Desarrollo de software", perfilGuardado.Descripcion);
        repository.Verify(repo => repo.ObtenerPorUsuarioIdAsync(usuarioAutenticadoId), Times.Once);
    }
}
