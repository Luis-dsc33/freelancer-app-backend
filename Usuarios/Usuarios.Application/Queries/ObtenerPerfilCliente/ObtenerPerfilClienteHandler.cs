using MediatR;
using Usuarios.Application.Interfaces;

namespace Usuarios.Application.Queries.ObtenerPerfilCliente;

public sealed class ObtenerPerfilClienteHandler(
    IPerfilClienteRepository perfiles,
    ICurrentUserService currentUser)
    : IRequestHandler<ObtenerPerfilClienteQuery, PerfilClienteDto?>
{
    public async Task<PerfilClienteDto?> Handle(
        ObtenerPerfilClienteQuery request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

        var perfil = await perfiles.ObtenerPorUsuarioIdAsync(currentUser.UserId);
        if (perfil is null)
            return null;

        return new PerfilClienteDto
        {
            Id = perfil.Id,
            UsuarioId = perfil.UsuarioId,
            NombreEmpresa = perfil.NombreEmpresa,
            Telefono = perfil.Telefono,
            Ubicacion = perfil.Ubicacion,
            Descripcion = perfil.Descripcion,
            DatosContacto = perfil.DatosContacto
        };
    }
}
