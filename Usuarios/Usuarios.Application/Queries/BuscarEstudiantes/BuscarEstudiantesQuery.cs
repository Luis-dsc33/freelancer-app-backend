using MediatR;

namespace Usuarios.Application.Queries.BuscarEstudiantes;

public sealed record BuscarEstudiantesQuery(
    string? Habilidad,
    string? Carrera,
    int Pagina = 1,
    int TamanoPagina = 20) : IRequest<IReadOnlyList<EstudiantePublicoDto>>;
