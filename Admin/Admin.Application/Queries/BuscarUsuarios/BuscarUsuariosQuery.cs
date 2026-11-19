// ADMIN Marcos: HU-25 Consultar usuarios. Los datos los entrega el servicio de Usuarios por HTTP.
using Admin.Application.Dtos;
using Admin.Application.Interfaces;
using MediatR;

namespace Admin.Application.Queries.BuscarUsuarios;

public record BuscarUsuariosQuery(string? Busqueda, string? Rol, string? Estado, int Pagina = 1, int TamanoPagina = 10)
    : IRequest<PaginaUsuariosDto>;

public class BuscarUsuariosHandler(IUsuariosAdminClient usuarios) : IRequestHandler<BuscarUsuariosQuery, PaginaUsuariosDto>
{
    public Task<PaginaUsuariosDto> Handle(BuscarUsuariosQuery request, CancellationToken ct) =>
        usuarios.BuscarAsync(request.Busqueda, request.Rol, request.Estado, request.Pagina, request.TamanoPagina, ct);
}
