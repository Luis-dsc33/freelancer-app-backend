// ADMIN Marcos: HU-25-02 historial de medidas de un usuario (o de una oferta), desde la bitácora propia.
using Admin.Application.Commands.AplicarMedidaModeracion;
using Admin.Application.Dtos;
using Admin.Application.Interfaces;
using MediatR;

namespace Admin.Application.Queries.ObtenerHistorialRecurso;

public record ObtenerHistorialRecursoQuery(Guid RecursoId) : IRequest<IReadOnlyList<AccionDto>>;

public class ObtenerHistorialRecursoHandler(IAccionAdministrativaRepository bitacora)
    : IRequestHandler<ObtenerHistorialRecursoQuery, IReadOnlyList<AccionDto>>
{
    public async Task<IReadOnlyList<AccionDto>> Handle(ObtenerHistorialRecursoQuery request, CancellationToken ct) =>
        (await bitacora.ObtenerPorRecursoAsync(request.RecursoId, ct)).Select(AplicarMedidaModeracionHandler.ADto).ToList();
}
