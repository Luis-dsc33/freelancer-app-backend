using MediatR;

namespace Usuarios.Application.Queries.ObtenerPerfil;

public record ObtenerPerfilQuery(Guid UsuarioId) : IRequest<PerfilEstudianteDto?>;
