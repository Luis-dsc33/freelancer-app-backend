using Moq;
using Usuarios.Application.Interfaces;
using Usuarios.Application.Queries.ObtenerPerfil;
using Usuarios.Domain.Entities;

namespace Usuarios.Tests.Queries;

public class ObtenerPerfilHandlerTests
{
    [Fact]
    public async Task Handle_PerfilInexistente_DevuelveIncompletoConCamposFaltantesYNombre()
    {
        var usuarioId = Guid.NewGuid();
        var repo = new Mock<IPerfilEstudianteRepository>();
        var usuarios = new Mock<IUsuarioRepository>();
        var currentUser = new Mock<ICurrentUserService>();

        currentUser.SetupGet(x => x.IsAuthenticated).Returns(true);
        currentUser.SetupGet(x => x.UserId).Returns(usuarioId);
        usuarios.Setup(x => x.ObtenerPorIdAsync(usuarioId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Usuario { Id = usuarioId, Nombre = "Luis", Email = "luis@utsjr.edu.mx", PasswordHash = "h", RolId = Guid.NewGuid() });
        repo.Setup(x => x.ObtenerPorUsuarioIdAsync(usuarioId)).ReturnsAsync((PerfilEstudiante?)null);

        var handler = new ObtenerPerfilHandler(repo.Object, usuarios.Object, currentUser.Object);

        var dto = await handler.Handle(new ObtenerPerfilQuery(), CancellationToken.None);

        Assert.Null(dto.Id);
        Assert.Equal(usuarioId, dto.UsuarioId);
        Assert.Equal("Luis", dto.Nombre);
        Assert.False(dto.EsPerfilCompleto);
        Assert.Equal(3, dto.CamposFaltantes.Count);
    }

    [Fact]
    public async Task Handle_PerfilConSkillsCatalogo_DevuelveCompletoConSkillsMapeadas()
    {
        var usuarioId = Guid.NewGuid();
        var perfilId = Guid.NewGuid();
        var skillId = Guid.NewGuid();
        var perfil = new PerfilEstudiante
        {
            Id = perfilId,
            UsuarioId = usuarioId,
            Carrera = "Software",
            Descripcion = "Fullstack",
            Habilidades = ["Angular"]
        };
        perfil.ActualizarSkillsCatalogo([
            new EstudianteSkill(perfilId, skillId, "Angular", "Frontend", "Avanzado")
        ]);

        var repo = new Mock<IPerfilEstudianteRepository>();
        var usuarios = new Mock<IUsuarioRepository>();
        var currentUser = new Mock<ICurrentUserService>();

        currentUser.SetupGet(x => x.IsAuthenticated).Returns(true);
        currentUser.SetupGet(x => x.UserId).Returns(usuarioId);
        usuarios.Setup(x => x.ObtenerPorIdAsync(usuarioId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Usuario { Id = usuarioId, Nombre = "Luis", Email = "luis@utsjr.edu.mx", PasswordHash = "h", RolId = Guid.NewGuid() });
        repo.Setup(x => x.ObtenerPorUsuarioIdAsync(usuarioId)).ReturnsAsync(perfil);

        var handler = new ObtenerPerfilHandler(repo.Object, usuarios.Object, currentUser.Object);

        var dto = await handler.Handle(new ObtenerPerfilQuery(), CancellationToken.None);

        Assert.True(dto.EsPerfilCompleto);
        Assert.Empty(dto.CamposFaltantes);
        Assert.Single(dto.SkillsCatalogo);
        Assert.Equal(skillId, dto.SkillsCatalogo[0].SkillId);
        Assert.Equal("Avanzado", dto.SkillsCatalogo[0].NivelDominio);
    }
}
