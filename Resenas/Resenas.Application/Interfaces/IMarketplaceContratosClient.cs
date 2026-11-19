namespace Resenas.Application.Interfaces;

public sealed record ContratoResumenDto(
    Guid Id,
    Guid PostulacionId,
    Guid ProyectoId,
    Guid ClienteId,
    Guid EstudianteId,
    string Estado,
    string? TituloProyecto,
    string? NombreCliente);

public interface IMarketplaceContratosClient
{
    Task<ContratoResumenDto?> ObtenerContratoAsync(Guid contratoId, CancellationToken cancellationToken);
}
