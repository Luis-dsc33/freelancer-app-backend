// ADMIN Marcos: HU-21 / HU-26. El login ya rechaza cuentas con Estado distinto de "Activo" (HU-02-03).
using MediatR;
using Usuarios.Application.Exceptions;
using Usuarios.Application.Interfaces;
using Usuarios.Application.Queries.BuscarUsuariosAdmin;

namespace Usuarios.Application.Commands.CambiarEstadoUsuario;

public class CambiarEstadoUsuarioHandler(IUsuarioAdminRepository repositorio)
    : IRequestHandler<CambiarEstadoUsuarioCommand, UsuarioAdminDto>
{
    public const string EstadoActivo = "Activo";
    public const string EstadoSuspendido = "Suspendido";

    public async Task<UsuarioAdminDto> Handle(CambiarEstadoUsuarioCommand request, CancellationToken ct)
    {
        var usuario = await repositorio.ObtenerParaActualizarAsync(request.UsuarioId, ct)
                      ?? throw new RecursoNoEncontradoException("El usuario no existe.");

        if (request.Suspender)
        {
            // HU-21-06: no se suspende a un Administrador (incluida la propia cuenta)
            if (usuario.Rol.Nombre == "Administrador")
                throw new InvalidOperationException("No se puede suspender una cuenta de Administrador.");
            // HU-21-05: medida ya vigente
            if (usuario.Estado == EstadoSuspendido)
                throw new InvalidOperationException("La cuenta ya está suspendida.");
            usuario.Estado = EstadoSuspendido;
            usuario.MotivoSuspension = string.IsNullOrWhiteSpace(request.Motivo) ? null : request.Motivo.Trim();
        }
        else
        {
            // HU-26-04: no hay medida vigente que revertir
            if (usuario.Estado == EstadoActivo)
                throw new InvalidOperationException("La cuenta ya está activa.");
            usuario.Estado = EstadoActivo;
            usuario.MotivoSuspension = null; // ADMIN Marcos: ya no hay suspensión vigente
        }

        await repositorio.GuardarCambiosAsync(ct);
        return BuscarUsuariosAdminHandler.ADto(usuario);
    }
}
