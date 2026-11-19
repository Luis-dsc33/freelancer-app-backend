using MediatR;
using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.Contratos;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Commands.CambiarEstadoContrato;

public sealed class CambiarEstadoContratoHandler(
    IContratoRepository contratos,
    ICurrentUserService currentUser)
    : IRequestHandler<CambiarEstadoContratoCommand, ContratoDto>
{
    public async Task<ContratoDto> Handle(CambiarEstadoContratoCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al cliente autenticado.");

        var contrato = await contratos.ObtenerPorIdAsync(request.ContratoId, cancellationToken)
            ?? throw new KeyNotFoundException("El contrato no existe.");
        if (contrato.ClienteId != currentUser.UserId)
            throw new UnauthorizedAccessException("El contrato no pertenece al cliente autenticado.");
        if (contrato.Estado != Contrato.EstadoActivo)
            throw new InvalidOperationException("El contrato ya no está activo.");
        if (request.Estado is not (Contrato.EstadoFinalizado or Contrato.EstadoCancelado))
            throw new InvalidOperationException("Estado de contrato no permitido.");

        contrato.Estado = request.Estado;
        contrato.CerradoEn = DateTime.UtcNow;
        await contratos.ActualizarAsync(contrato, cancellationToken);
        return ContratoMapper.ToDto(contrato);
    }
}
