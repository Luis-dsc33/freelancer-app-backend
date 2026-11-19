using Marketplace.Application.Interfaces;
using MediatR;

namespace Marketplace.Application.Queries.Contratos;

public sealed class ListarMisContratosHandler(
    IContratoRepository contratos,
    ICurrentUserService currentUser)
    : IRequestHandler<ListarMisContratosQuery, IReadOnlyList<ContratoDto>>
{
    public async Task<IReadOnlyList<ContratoDto>> Handle(
        ListarMisContratosQuery request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

        if (!currentUser.EsEstudiante && !currentUser.EsCliente)
            throw new UnauthorizedAccessException("Solo clientes y estudiantes pueden consultar sus contratos.");

        var lista = currentUser.EsEstudiante
            ? await contratos.ObtenerPorEstudianteAsync(currentUser.UserId, cancellationToken)
            : await contratos.ObtenerPorClienteAsync(currentUser.UserId, cancellationToken);

        return lista.Select(ContratoMapper.ToDto).ToList();
    }
}
