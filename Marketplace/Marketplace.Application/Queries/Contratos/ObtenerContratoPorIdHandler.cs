using Marketplace.Application.Interfaces;
using MediatR;

namespace Marketplace.Application.Queries.Contratos;

public sealed class ObtenerContratoPorIdHandler(
    IContratoRepository contratos,
    ICurrentUserService currentUser)
    : IRequestHandler<ObtenerContratoPorIdQuery, ContratoDto>
{
    public async Task<ContratoDto> Handle(
        ObtenerContratoPorIdQuery request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

        var contrato = await contratos.ObtenerPorIdAsync(request.ContratoId, cancellationToken)
            ?? throw new KeyNotFoundException("El contrato no existe.");

        if (contrato.ClienteId != currentUser.UserId && contrato.EstudianteId != currentUser.UserId)
            throw new UnauthorizedAccessException("No tienes permiso para acceder a este contrato.");

        return ContratoMapper.ToDto(contrato);
    }
}
