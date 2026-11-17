using MediatR;
using Usuarios.Application.Interfaces;

namespace Usuarios.Application.Queries.BuscarEstudiantes;

public sealed class BuscarEstudiantesHandler(IPerfilEstudianteRepository perfiles)
    : IRequestHandler<BuscarEstudiantesQuery, IReadOnlyList<EstudiantePublicoDto>>
{
    public async Task<IReadOnlyList<EstudiantePublicoDto>> Handle(
        BuscarEstudiantesQuery request,
        CancellationToken cancellationToken)
    {
        var perfilesPublicos = await perfiles.BuscarPublicosAsync(
            request.Habilidad,
            request.Carrera,
            request.Pagina,
            request.TamanoPagina,
            cancellationToken);

        return perfilesPublicos
            .Select(p => new EstudiantePublicoDto(
                p.UsuarioId,
                p.Carrera,
                p.Habilidades,
                p.Descripcion))
            .ToList();
    }
}
