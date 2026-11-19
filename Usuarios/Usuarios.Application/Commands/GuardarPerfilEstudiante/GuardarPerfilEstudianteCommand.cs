using MediatR;
using Usuarios.Application.Queries.ObtenerPerfil;

namespace Usuarios.Application.Commands.GuardarPerfilEstudiante;

public sealed record GuardarPerfilEstudianteCommand(
    string Carrera,
    string Descripcion,
    List<string> Habilidades,
    List<EstudianteSkillInputDto>? SkillsCatalogo = null) : IRequest<PerfilEstudianteDto>;
