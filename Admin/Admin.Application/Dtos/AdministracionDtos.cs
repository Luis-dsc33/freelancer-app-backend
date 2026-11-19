// ADMIN Marcos: contratos de lectura de las secciones Usuarios, Moderación y Bitácora (HU-25, HU-21/26, HU-27).
namespace Admin.Application.Dtos;

// Datos que entrega el servicio de Usuarios (nunca incluye contraseñas)
public record UsuarioDto(Guid Id, string Nombre, string Email, string Rol, string Estado, DateTime FechaRegistro);
public record PaginaUsuariosDto(IReadOnlyList<UsuarioDto> Items, int Total, int Pagina, int TamanoPagina);

// Datos que entrega el servicio de Marketplace
public record OfertaDto(
    Guid Id, string Titulo, string Disciplina, string NombreCliente, decimal Presupuesto, string Estado, DateTime CreadoEn);

// Bitácora
public record AccionDto(
    Guid Id, Guid AdminId, string AdminNombre, string TipoAccion, string RecursoTipo,
    Guid RecursoId, string RecursoNombre, string Motivo, DateTime Fecha);

public record AdministradorDto(Guid Id, string Nombre);

public record BitacoraDto(
    IReadOnlyList<AccionDto> Items, int Total, int Pagina, int TamanoPagina, IReadOnlyList<AdministradorDto> Administradores);

// Moderación: medida vigente con su motivo (sale de la bitácora)
public record CuentaSuspendidaDto(UsuarioDto Usuario, AccionDto? Medida);
public record OfertaModeracionDto(OfertaDto Oferta, AccionDto? Medida);

// Cada lista es null si su servicio no respondió (se muestra "No disponible")
public record ModeracionDto(IReadOnlyList<CuentaSuspendidaDto>? CuentasSuspendidas, IReadOnlyList<OfertaModeracionDto>? Ofertas);
