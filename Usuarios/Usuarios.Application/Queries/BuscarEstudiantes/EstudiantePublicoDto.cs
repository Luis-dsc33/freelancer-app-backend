namespace Usuarios.Application.Queries.BuscarEstudiantes;

public sealed record EstudiantePublicoDto(
    Guid UsuarioId,
    string Carrera,
    IReadOnlyList<string> Habilidades,
    string Descripcion);
