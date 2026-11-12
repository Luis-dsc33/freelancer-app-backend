namespace Usuarios.Application.Queries.ObtenerPortafolio;

public record TrabajoPortafolioDto(Guid Id, string Titulo, string Descripcion, string? Enlace,
    DateTime CreadoEn, DateTime? ActualizadoEn);
