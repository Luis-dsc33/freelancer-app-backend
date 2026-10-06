using MediatR;
using Usuarios.Application.Interfaces;
using Usuarios.Application.Exceptions;

namespace Usuarios.Application.Queries.ObtenerPortafolio;

public class ObtenerPortafolioHandler(ITrabajoPortafolioRepository trabajos, IUsuarioRepository usuarios)
    : IRequestHandler<ObtenerPortafolioQuery, List<TrabajoPortafolioDto>>
{
    public async Task<List<TrabajoPortafolioDto>> Handle(ObtenerPortafolioQuery request, CancellationToken cancellationToken)
    {
        var usuario = await usuarios.ObtenerPorIdAsync(request.UsuarioId, cancellationToken);
        if (usuario is null || usuario.Rol.Nombre != "Estudiante" || usuario.Estado != "Activo")
            throw new RecursoNoEncontradoException("No se encontró el portafolio del estudiante.");

        var lista = await trabajos.ObtenerPorUsuarioAsync(request.UsuarioId, cancellationToken);
        return lista.Select(t => new TrabajoPortafolioDto(t.Id, t.Titulo, t.Descripcion, t.Enlace,
            t.CreadoEn, t.ActualizadoEn)).ToList();
    }
}
