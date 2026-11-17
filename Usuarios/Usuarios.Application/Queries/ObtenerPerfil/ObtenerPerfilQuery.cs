using MediatR;

namespace Usuarios.Application.Queries.ObtenerPerfil;

public record ObtenerPerfilQuery : IRequest<PerfilEstudianteDto>;