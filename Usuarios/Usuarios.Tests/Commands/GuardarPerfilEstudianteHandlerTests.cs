using FluentValidation.TestHelper;
using Moq;
using Usuarios.Application.Commands.GuardarPerfilEstudiante;
using Usuarios.Application.Interfaces;
using Usuarios.Application.Queries.ObtenerPerfil;
using Usuarios.Domain.Entities;

namespace Usuarios.Tests.Commands;

public class GuardarPerfilEstudianteHandlerTests
{
    [Fact]
    public void Validator_SinHabilidades_DevuelveError()
    {
        var validator = new GuardarPerfilEstudianteCommandValidator();
        var cmd = new GuardarPerfilEstudianteCommand("Ingeniería", "Descripción válida", [], []);

        var resultado = validator.TestValidate(cmd);

        Assert.False(resultado.IsValid);
    }

    [Fact]
    public void Validator_NivelDominioInvalido_DevuelveError()
    {
        var validator = new GuardarPerfilEstudianteCommandValidator();
        var cmd = new GuardarPerfilEstudianteCommand(
            "Ingeniería",
            "Descripción válida",
            [],
            [new EstudianteSkillInputDto(Guid.NewGuid(), "C#", "Experto")]);

        var resultado = validator.TestValidate(cmd);

        Assert.False(resultado.IsValid);
    }

    [Fact]
    public async Task Handle_SkillsCatalogoValidadas_PersisteNormalizadasYSincronizaHabilidades()
    {
        var usuarioId = Guid.NewGuid();
        var skillId = Guid.NewGuid();
        var repo = new Mock<IPerfilEstudianteRepository>();
        var usuariosRepo = new Mock<IUsuarioRepository>();
        var catalogoClient = new Mock<ICatalogoSkillsMarketplaceClient>();
        var currentUser = new Mock<ICurrentUserService>();

        currentUser.SetupGet(x => x.IsAuthenticated).Returns(true);
        currentUser.SetupGet(x => x.UserId).Returns(usuarioId);
        currentUser.SetupGet(x => x.Role).Returns("Estudiante");
        currentUser.SetupGet(x => x.EsEstudiante).Returns(true);

        usuariosRepo
            .Setup(x => x.ObtenerPorIdAsync(usuarioId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Usuario { Id = usuarioId, Nombre = "Ana López", Email = "ana@utsjr.edu.mx", PasswordHash = "hash", RolId = Guid.NewGuid() });

        repo.Setup(x => x.ObtenerPorUsuarioIdAsync(usuarioId)).ReturnsAsync((PerfilEstudiante?)null);

        PerfilEstudiante? guardado = null;
        repo.Setup(x => x.AgregarAsync(It.IsAny<PerfilEstudiante>()))
            .Callback<PerfilEstudiante>(p => guardado = p)
            .Returns(Task.CompletedTask);

        catalogoClient
            .Setup(x => x.ValidarYObtenerSkillsAsync(It.IsAny<IReadOnlyCollection<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<Guid, SkillCatalogoValidadaDto>
            {
                [skillId] = new SkillCatalogoValidadaDto(skillId, "Angular", "Desarrollo frontend")
            });

        var handler = new GuardarPerfilEstudianteHandler(repo.Object, usuariosRepo.Object, catalogoClient.Object, currentUser.Object);

        var dto = await handler.Handle(
            new GuardarPerfilEstudianteCommand(
                "Ingeniería de Software",
                "Estudiante apasionada por Angular y .NET",
                ["Scrum"],
                [new EstudianteSkillInputDto(skillId, "angular-cliente", "Avanzado")]),
            CancellationToken.None);

        Assert.NotNull(guardado);
        Assert.Equal(usuarioId, guardado!.UsuarioId);
        Assert.Equal("Ana López", dto.Nombre);
        Assert.True(dto.EsPerfilCompleto);
        Assert.Empty(dto.CamposFaltantes);
        Assert.Single(dto.SkillsCatalogo);
        Assert.Equal("Angular", dto.SkillsCatalogo[0].NombreSkill);
        Assert.Equal("Desarrollo frontend", dto.SkillsCatalogo[0].Categoria);
        Assert.Equal("Avanzado", dto.SkillsCatalogo[0].NivelDominio);
        Assert.Contains("Angular", dto.Habilidades);
        Assert.Contains("Scrum", dto.Habilidades);
    }

    [Fact]
    public async Task Handle_SkillCatalogoInexistente_LanzaInvalidOperationExceptionYNoGuarda()
    {
        var usuarioId = Guid.NewGuid();
        var repo = new Mock<IPerfilEstudianteRepository>();
        var usuariosRepo = new Mock<IUsuarioRepository>();
        var catalogoClient = new Mock<ICatalogoSkillsMarketplaceClient>();
        var currentUser = new Mock<ICurrentUserService>();

        currentUser.SetupGet(x => x.IsAuthenticated).Returns(true);
        currentUser.SetupGet(x => x.UserId).Returns(usuarioId);
        currentUser.SetupGet(x => x.Role).Returns("Estudiante");
        currentUser.SetupGet(x => x.EsEstudiante).Returns(true);

        catalogoClient
            .Setup(x => x.ValidarYObtenerSkillsAsync(It.IsAny<IReadOnlyCollection<Guid>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("La habilidad no existe."));

        var handler = new GuardarPerfilEstudianteHandler(repo.Object, usuariosRepo.Object, catalogoClient.Object, currentUser.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(
                new GuardarPerfilEstudianteCommand(
                    "Ingeniería",
                    "Descripción",
                    [],
                    [new EstudianteSkillInputDto(Guid.NewGuid(), "Fake", "Basico")]),
                CancellationToken.None));

        repo.Verify(x => x.AgregarAsync(It.IsAny<PerfilEstudiante>()), Times.Never);
        repo.Verify(x => x.ActualizarAsync(It.IsAny<PerfilEstudiante>()), Times.Never);
    }
}
