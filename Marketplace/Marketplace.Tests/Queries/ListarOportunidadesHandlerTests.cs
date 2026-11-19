using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.ListarOportunidades;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Enums;
using Moq;

namespace Marketplace.Tests.Queries;

public class ListarOportunidadesHandlerTests
{
    [Fact]
    public async Task Handle_MapeaSkillsDeseablesYMarcaYaPostuladoEnLote()
    {
        var estudianteId = Guid.NewGuid();
        var proyectoPostulado = CrearProyecto("App 1", TipoRequisitoSkill.Requerida, "Angular");
        var proyectoLibre = CrearProyecto("App 2", TipoRequisitoSkill.Deseable, "Docker");

        var proyectosRepo = new Mock<IProyectoRepository>();
        var postulacionesRepo = new Mock<IPostulacionRepository>();
        var currentUser = new Mock<ICurrentUserService>();

        currentUser.SetupGet(x => x.IsAuthenticated).Returns(true);
        currentUser.SetupGet(x => x.UserId).Returns(estudianteId);
        currentUser.SetupGet(x => x.Role).Returns("Estudiante");
        currentUser.SetupGet(x => x.EsEstudiante).Returns(true);

        proyectosRepo
            .Setup(x => x.BuscarAbiertosAsync(It.IsAny<FiltroOportunidadesProyecto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([proyectoPostulado, proyectoLibre]);

        postulacionesRepo
            .Setup(x => x.ObtenerProyectosPostuladosPorEstudianteAsync(
                estudianteId,
                It.IsAny<IEnumerable<Guid>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([proyectoPostulado.Id]);

        var handler = new ListarOportunidadesHandler(proyectosRepo.Object, postulacionesRepo.Object, currentUser.Object);

        var resultado = await handler.Handle(new ListarOportunidadesQuery(Busqueda: "App"), CancellationToken.None);

        Assert.Equal(2, resultado.Count);
        Assert.True(resultado[0].YaPostulado);
        Assert.False(resultado[1].YaPostulado);
        Assert.NotNull(resultado[0].HabilidadesRequeridas);
        Assert.NotNull(resultado[0].HabilidadesDeseables);
        Assert.Single(resultado[0].Skills);
        Assert.Equal("Angular", resultado[0].Skills[0].NombreSkill);
        Assert.Contains("Docker", resultado[1].HabilidadesDeseables);
    }

    private static Proyecto CrearProyecto(string titulo, TipoRequisitoSkill tipo, string nombreSkill)
    {
        var id = Guid.NewGuid();
        var skillId = Guid.NewGuid();
        return new Proyecto
        {
            Id = id,
            Titulo = titulo,
            Descripcion = "Descripción",
            Disciplina = "Desarrollo de software",
            Presupuesto = 5000m,
            FechaLimite = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(10),
            ClienteId = Guid.NewGuid(),
            NombreCliente = "Cliente Demo",
            Estado = Proyecto.EstadoPublicada,
            CreadoEn = DateTime.UtcNow,
            Skills =
            [
                new ProyectoSkill
                {
                    ProyectoId = id,
                    SkillId = skillId,
                    TipoRequisito = tipo,
                    Skill = new Skill { Id = skillId, Nombre = nombreSkill }
                }
            ]
        };
    }
}
