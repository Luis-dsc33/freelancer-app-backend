using MediatR;
using Resenas.Application.Dtos;
using Resenas.Application.Interfaces;

namespace Resenas.Application.Queries.ListarMisResenasEmitidas;

public sealed class ListarMisResenasEmitidasHandler(
    IResenaRepository resenas,
    ICurrentUserService currentUser)
    : IRequestHandler<ListarMisResenasEmitidasQuery, IReadOnlyList<ResenaDto>>
{
    public async Task<IReadOnlyList<ResenaDto>> Handle(
        ListarMisResenasEmitidasQuery request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

        var lista = await resenas.ListarPorAutorAsync(currentUser.UserId, cancellationToken);

        return lista.Select(r => new ResenaDto(
            r.Id,
            r.ContratoId,
            r.AutorId,
            r.DestinatarioId,
            r.Puntuacion,
            r.Comentario,
            DateTime.SpecifyKind(r.CreadaEn, DateTimeKind.Utc))).ToList();
    }
}
