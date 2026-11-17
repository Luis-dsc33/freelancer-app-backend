using MediatR;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Application.Commands.CrearOActualizarPerfilCliente;

public sealed class CrearOActualizarPerfilClienteHandler(
    IPerfilClienteRepository perfiles,
    IUsuarioRepository usuarios,
    ICurrentUserService currentUser)
    : IRequestHandler<CrearOActualizarPerfilClienteCommand, Guid>
{
    public async Task<Guid> Handle(
        CrearOActualizarPerfilClienteCommand request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

        var usuarioId = currentUser.UserId;
        var usuario = await usuarios.ObtenerPorIdAsync(usuarioId, cancellationToken);
        if (usuario is null || !string.Equals(usuario.Rol.Nombre, "Cliente", StringComparison.OrdinalIgnoreCase))
            throw new UnauthorizedAccessException("El usuario autenticado no es un cliente válido.");

        var perfil = await perfiles.ObtenerPorUsuarioIdAsync(usuarioId);
        if (perfil is null)
        {
            perfil = new PerfilCliente
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuarioId
            };

            AsignarDatos(perfil, request);
            await perfiles.AgregarAsync(perfil);
            return perfil.Id;
        }

        AsignarDatos(perfil, request);
        await perfiles.ActualizarAsync(perfil);
        return perfil.Id;
    }

    private static void AsignarDatos(
        PerfilCliente perfil,
        CrearOActualizarPerfilClienteCommand request)
    {
        perfil.NombreEmpresa = request.NombreEmpresa.Trim();
        perfil.Telefono = request.Telefono.Trim();
        perfil.Ubicacion = request.Ubicacion.Trim();
        perfil.Descripcion = request.Descripcion.Trim();
        perfil.DatosContacto = request.DatosContacto.Trim();
    }
}
