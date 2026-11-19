// ADMIN Marcos: HU-25
using MediatR;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Application.Queries.BuscarUsuariosAdmin;

public class BuscarUsuariosAdminHandler(IUsuarioAdminRepository repositorio)
    : IRequestHandler<BuscarUsuariosAdminQuery, PaginaUsuariosAdminDto>
{
    public async Task<PaginaUsuariosAdminDto> Handle(BuscarUsuariosAdminQuery request, CancellationToken ct)
    {
        var pagina = Math.Max(1, request.Pagina);
        var tamano = Math.Clamp(request.TamanoPagina, 1, 50);

        var (usuarios, total) = await repositorio.BuscarAsync(
            request.Busqueda, request.Rol, request.Estado, pagina, tamano, ct);

        return new PaginaUsuariosAdminDto(usuarios.Select(ADto).ToList(), total, pagina, tamano);
    }

    public static UsuarioAdminDto ADto(Usuario u) =>
        new(u.Id, u.Nombre, u.Email, u.Rol.Nombre, u.Estado, DateTime.SpecifyKind(u.FechaRegistro, DateTimeKind.Utc));
}
