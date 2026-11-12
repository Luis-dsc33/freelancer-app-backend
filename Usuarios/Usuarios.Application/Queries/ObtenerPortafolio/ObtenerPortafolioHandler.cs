using MediatR;
using Usuarios.Application.Interfaces;
using Usuarios.Application.Exceptions;

namespace Usuarios.Application.Queries.ObtenerPortafolio;

public class ObtenerPortafolioHandler(
    ITrabajoPortafolioRepository trabajos,
    IUsuarioRepository usuarios,
    ICurrentUserService currentUser)
    : IRequestHandler<ObtenerPortafolioQuery, List<TrabajoPortafolioDto>>,
      IRequestHandler<ObtenerMiPortafolioQuery, List<TrabajoPortafolioDto>>
{
    public Task<List<TrabajoPortafolioDto>> Handle(
        ObtenerPortafolioQuery request,
        CancellationToken cancellationToken) =>
        ObtenerAsync(request.UsuarioId, cancellationToken);

    public Task<List<TrabajoPortafolioDto>> Handle(
        ObtenerMiPortafolioQuery request,
        CancellationToken cancellationToken) =>
        ObtenerAsync(currentUser.UsuarioId, cancellationToken);

    private async Task<List<TrabajoPortafolioDto>> ObtenerAsync(
        Guid usuarioId,
        CancellationToken cancellationToken)
    {
        var usuario = await usuarios.ObtenerPorIdAsync(usuarioId, cancellationToken);
        if (usuario is null || usuario.Rol.Nombre != "Estudiante" || usuario.Estado != "Activo")
            throw new RecursoNoEncontradoException("No se encontró el portafolio del estudiante.");

        var lista = await trabajos.ObtenerPorUsuarioAsync(usuarioId, cancellationToken);
        return lista.Select(t => new TrabajoPortafolioDto(t.Id, t.Titulo, t.Descripcion, t.Enlace,
            t.CreadoEn, t.ActualizadoEn)).ToList();
    }
}
