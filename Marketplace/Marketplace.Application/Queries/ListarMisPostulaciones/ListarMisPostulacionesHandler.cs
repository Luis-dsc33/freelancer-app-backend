using Marketplace.Application.Interfaces;
using MediatR;

namespace Marketplace.Application.Queries.ListarMisPostulaciones;

public sealed class ListarMisPostulacionesHandler(
    IPostulacionRepository postulaciones,
    IContratoRepository contratos,
    ICurrentUserService currentUser)
    : IRequestHandler<ListarMisPostulacionesQuery, IReadOnlyList<PostulacionEstudianteDto>>
{
    public async Task<IReadOnlyList<PostulacionEstudianteDto>> Handle(
        ListarMisPostulacionesQuery request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al estudiante autenticado.");

        if (!currentUser.EsEstudiante)
            throw new UnauthorizedAccessException("Solo las cuentas de estudiante pueden consultar sus postulaciones.");

        var lista = await postulaciones.ListarPorEstudianteAsync(currentUser.UserId, cancellationToken);
        var contratosPorPostulacion = lista.Count > 0
            ? await contratos.ObtenerContratosPorPostulacionesAsync(lista.Select(p => p.Id), cancellationToken)
            : new Dictionary<Guid, Guid>();

        return lista.Select(p =>
        {
            contratosPorPostulacion.TryGetValue(p.Id, out var contratoId);
            var presupuesto = p.Proyecto.Presupuesto > 0
                ? p.Proyecto.Presupuesto
                : p.Proyecto.PresupuestoMaximo ?? p.Proyecto.PresupuestoMinimo ?? 0m;

            return new PostulacionEstudianteDto(
                p.Id,
                p.ProyectoId,
                p.EstudianteId,
                p.Mensaje,
                p.Estado,
                DateTime.SpecifyKind(p.CreadaEn, DateTimeKind.Utc),
                p.ResueltaEn.HasValue ? DateTime.SpecifyKind(p.ResueltaEn.Value, DateTimeKind.Utc) : null,
                p.Proyecto.Titulo,
                p.Proyecto.Disciplina,
                p.Proyecto.Modalidad.ToString(),
                presupuesto,
                p.Proyecto.NombreCliente,
                p.Proyecto.Estado,
                contratoId == Guid.Empty ? null : contratoId);
        }).ToList();
    }
}
