using MediatR;
using Marketplace.Application.Interfaces;

namespace Marketplace.Application.Queries.Contratos;

public sealed class ListarContratosClienteHandler(
    IContratoRepository contratos,
    ICurrentUserService currentUser)
    : IRequestHandler<ListarContratosClienteQuery, IReadOnlyList<ContratoDto>>
{
    public async Task<IReadOnlyList<ContratoDto>> Handle(ListarContratosClienteQuery request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al cliente autenticado.");

        var contratosCliente = await contratos.ObtenerPorClienteAsync(currentUser.UserId, cancellationToken);
        return contratosCliente.Select(ContratoMapper.ToDto).ToList();
    }
}
