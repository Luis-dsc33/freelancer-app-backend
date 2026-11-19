// ADMIN Marcos: HU-25 Consultar usuarios (búsqueda paginada para el Administrador).
using MediatR;

namespace Usuarios.Application.Queries.BuscarUsuariosAdmin;

public record BuscarUsuariosAdminQuery(
    string? Busqueda, string? Rol, string? Estado, int Pagina = 1, int TamanoPagina = 10)
    : IRequest<PaginaUsuariosAdminDto>;

// Nunca incluye la contraseña ni su hash (HU-22-01)
public record UsuarioAdminDto(Guid Id, string Nombre, string Email, string Rol, string Estado, DateTime FechaRegistro);

public record PaginaUsuariosAdminDto(IReadOnlyList<UsuarioAdminDto> Items, int Total, int Pagina, int TamanoPagina);
