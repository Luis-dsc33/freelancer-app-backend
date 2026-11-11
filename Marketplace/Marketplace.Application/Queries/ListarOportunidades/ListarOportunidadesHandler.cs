using Marketplace.Application.Interfaces;
using MediatR;

namespace Marketplace.Application.Queries.ListarOportunidades;

public class ListarOportunidadesHandler(IProyectoRepository proyectos)
    : IRequestHandler<ListarOportunidadesQuery, IReadOnlyList<OportunidadProyectoDto>>
{
    public async Task<IReadOnlyList<OportunidadProyectoDto>> Handle(
        ListarOportunidadesQuery request, CancellationToken cancellationToken)
    {
        // El repositorio ya devuelve solo proyectos "Abierto", ordenados por esUrgente DESC, creadoEn DESC.
        var lista = await proyectos.ObtenerAbiertosAsync(cancellationToken);

        return lista.Select(p => new OportunidadProyectoDto(
                p.Id,
                p.Titulo,
                p.Descripcion,
                p.Presupuesto,
                p.EsUrgente,
                (IReadOnlyList<string>)(p.HabilidadesRequeridas ?? new List<string>()).ToList(), // nunca null
                p.NombreCliente,
                p.Estado,
                DateTime.SpecifyKind(p.CreadoEn, DateTimeKind.Utc)))
            .ToList();
    }
}

