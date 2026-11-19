using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.ObtenerEstadisticasMarketplace;
using Marketplace.Domain.Entities;
using Moq;

namespace Marketplace.Tests.Queries;

public class ObtenerEstadisticasMarketplaceTests
{
    [Fact]
    public async Task CuentaOfertasDisponiblesYPorDisciplinaIncluyendoAbiertoYPublicada()
    {
        IReadOnlyCollection<string>? estadosContados = null;
        IReadOnlyCollection<string>? estadosDisciplina = null;

        var repositorio = new Mock<IProyectoAdminRepository>();
        repositorio
            .Setup(r => r.ContarPorEstadosAsync(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<CancellationToken>()))
            .Callback<IReadOnlyCollection<string>, CancellationToken>((estados, _) => estadosContados = estados)
            .ReturnsAsync(7);
        repositorio
            .Setup(r => r.ContarPorDisciplinaAsync(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<CancellationToken>()))
            .Callback<IReadOnlyCollection<string>, CancellationToken>((estados, _) => estadosDisciplina = estados)
            .ReturnsAsync(new Dictionary<string, int>
            {
                ["Desarrollo de software"] = 5,
                ["Diseño"] = 2
            });
        repositorio
            .Setup(r => r.FechasDeCreacionDesdeAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([DateTime.UtcNow]);
        repositorio
            .Setup(r => r.ContarPostulacionesPorEstadoAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<string, int>
            {
                [Postulacion.EstadoPendiente] = 4,
                [Postulacion.EstadoAceptada] = 2,
                [Postulacion.EstadoRechazada] = 1,
                [Postulacion.EstadoRetirada] = 1
            });

        var handler = new ObtenerEstadisticasMarketplaceHandler(repositorio.Object);

        var dto = await handler.Handle(new ObtenerEstadisticasMarketplaceQuery(), CancellationToken.None);

        Assert.Equal(7, dto.OfertasDisponibles);
        Assert.NotNull(estadosContados);
        Assert.Contains(Proyecto.EstadoAbierto, estadosContados!);
        Assert.Contains(Proyecto.EstadoPublicada, estadosContados!);
        Assert.NotNull(estadosDisciplina);
        Assert.Contains(Proyecto.EstadoAbierto, estadosDisciplina!);
        Assert.Contains(Proyecto.EstadoPublicada, estadosDisciplina!);
        Assert.Equal(2, dto.OfertasPorDisciplina.Count);
        Assert.Equal("Desarrollo de software", dto.OfertasPorDisciplina[0].Etiqueta);
        Assert.Equal(8, dto.Postulaciones.Enviadas);
        Assert.Equal(4, dto.Postulaciones.EnRevision);
        Assert.Equal(2, dto.Postulaciones.Aceptadas);
        Assert.Equal(2, dto.Postulaciones.NoSeleccionadas);
    }
}

