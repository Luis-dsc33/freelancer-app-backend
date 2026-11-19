using Marketplace.Application.Interfaces;
using MediatR;

namespace Marketplace.Application.Queries.ListarOportunidades;

public class ListarOportunidadesHandler(
    IProyectoRepository proyectos,
    IPostulacionRepository postulaciones,
    ICurrentUserService currentUser)
    : IRequestHandler<ListarOportunidadesQuery, IReadOnlyList<OportunidadProyectoDto>>
{
    public async Task<IReadOnlyList<OportunidadProyectoDto>> Handle(
        ListarOportunidadesQuery request,
        CancellationToken cancellationToken)
    {
        var pageNumber = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        var filtro = new FiltroOportunidadesProyecto(
            request.Busqueda,
            request.Disciplina,
            request.Modalidad,
            request.PresupuestoMin,
            request.PresupuestoMax,
            request.SoloUrgentes,
            request.SkillIds,
            request.Habilidad,
            pageNumber,
            pageSize);

        var lista = await proyectos.BuscarAbiertosAsync(filtro, cancellationToken);

        HashSet<Guid> proyectosPostulados = [];
        if (currentUser.IsAuthenticated && currentUser.UserId != Guid.Empty && currentUser.EsEstudiante && lista.Count > 0)
        {
            proyectosPostulados = await postulaciones.ObtenerProyectosPostuladosPorEstudianteAsync(
                currentUser.UserId,
                lista.Select(p => p.Id),
                cancellationToken);
        }

        return lista
            .Select(p => ProyectoMapper.ToDto(p, proyectosPostulados.Contains(p.Id)))
            .ToList();
    }
}
