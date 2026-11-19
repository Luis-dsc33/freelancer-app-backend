// ADMIN Marcos: HU-21 Suspender cuenta / HU-26 Reactivar cuenta.
// El motivo y quién lo hizo se registran en la bitácora del servicio Admin; aquí solo cambia el estado.
using MediatR;
using Usuarios.Application.Queries.BuscarUsuariosAdmin;

namespace Usuarios.Application.Commands.CambiarEstadoUsuario;

public record CambiarEstadoUsuarioCommand(Guid UsuarioId, bool Suspender) : IRequest<UsuarioAdminDto>;
