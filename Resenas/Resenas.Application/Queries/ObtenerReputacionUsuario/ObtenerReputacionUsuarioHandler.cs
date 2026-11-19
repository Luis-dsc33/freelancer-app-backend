using MediatR;
using Resenas.Application.Dtos;
using Resenas.Application.Interfaces;

namespace Resenas.Application.Queries.ObtenerReputacionUsuario;

public sealed class ObtenerReputacionUsuarioHandler(
    IResenaRepository resenas,
    IUsuariosPublicoClient? usuariosClient = null)
    : IRequestHandler<ObtenerReputacionUsuarioQuery, ReputacionUsuarioDto>
{
    public async Task<ReputacionUsuarioDto> Handle(
        ObtenerReputacionUsuarioQuery request,
        CancellationToken cancellationToken)
    {
        if (request.UsuarioId == Guid.Empty)
            throw new InvalidOperationException("El identificador del usuario es obligatorio.");

        var lista = await resenas.ListarPorDestinatarioAsync(request.UsuarioId, cancellationToken);

        IReadOnlyDictionary<Guid, string> nombres = new Dictionary<Guid, string>();
        if (lista.Count > 0 && usuariosClient is not null)
        {
            nombres = await usuariosClient.ObtenerNombresPorIdsAsync(
                lista.Select(r => r.AutorId).Distinct().ToList(),
                cancellationToken);
        }

        var promedio = lista.Count > 0
            ? Math.Round(lista.Average(r => r.Puntuacion), 2)
            : 0d;

        var items = lista.Select(r =>
        {
            nombres.TryGetValue(r.AutorId, out var nombreAutor);
            return new ResenaRecibidaDto(
                r.Id,
                r.ContratoId,
                r.AutorId,
                nombreAutor,
                r.Puntuacion,
                r.Comentario,
                DateTime.SpecifyKind(r.CreadaEn, DateTimeKind.Utc));
        }).ToList();

        return new ReputacionUsuarioDto(
            request.UsuarioId,
            promedio,
            lista.Count,
            items);
    }
}
