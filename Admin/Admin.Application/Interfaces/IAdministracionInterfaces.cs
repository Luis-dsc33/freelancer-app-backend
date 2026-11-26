// ADMIN Marcos: contratos de la capa Application para HU-25, HU-21/26 y HU-27.
using Admin.Application.Dtos;
using Admin.Domain.Entities;

namespace Admin.Application.Interfaces;

public interface IAccionAdministrativaRepository
{
    Task AgregarAsync(AccionAdministrativa accion, CancellationToken ct);

    Task<(List<AccionAdministrativa> Acciones, int Total)> BuscarAsync(
        Guid? adminId, string? tipoAccion, DateTime? desde, DateTime? hastaExclusivo, int pagina, int tamanoPagina, CancellationToken ct);

    Task<List<AccionAdministrativa>> ObtenerPorRecursoAsync(Guid recursoId, CancellationToken ct);

    // Última acción de cierto tipo por recurso (para mostrar el motivo de la medida vigente)
    Task<Dictionary<Guid, AccionAdministrativa>> ObtenerUltimasAsync(string tipoAccion, IReadOnlyCollection<Guid> recursoIds, CancellationToken ct);

    Task<List<AdministradorDto>> ObtenerAdministradoresAsync(CancellationToken ct);
}

// Administrador que hace la petición (sale del token JWT, nunca del body)
public interface IUsuarioActual
{
    Guid Id { get; }
    string Nombre { get; }
}

// Operaciones que Admin pide al servicio de Usuarios (dueño del dato)
public interface IUsuariosAdminClient
{
    Task<PaginaUsuariosDto> BuscarAsync(string? busqueda, string? rol, string? estado, int pagina, int tamanoPagina, CancellationToken ct);
    Task<UsuarioDto> SuspenderAsync(Guid usuarioId, string motivo, CancellationToken ct); // ADMIN Marcos: el motivo se muestra en el login
    Task<UsuarioDto> ReactivarAsync(Guid usuarioId, CancellationToken ct);
}

// Operaciones que Admin pide al servicio de Marketplace (dueño del dato)
public interface IMarketplaceAdminClient
{
    Task<IReadOnlyList<OfertaDto>> ListarOfertasAsync(string? estado, CancellationToken ct);
    Task<OfertaDto> OcultarAsync(Guid ofertaId, CancellationToken ct);
    Task<OfertaDto> RestaurarAsync(Guid ofertaId, CancellationToken ct);
}
