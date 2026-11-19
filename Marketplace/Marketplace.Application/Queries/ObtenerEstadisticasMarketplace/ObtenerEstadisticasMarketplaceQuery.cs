// ADMIN Marcos: HU-28 estadísticas que consume el panel del servicio Admin.
// La forma del JSON coincide con EstadisticasMarketplaceDto de Admin.Application.
using MediatR;

namespace Marketplace.Application.Queries.ObtenerEstadisticasMarketplace;

public record ObtenerEstadisticasMarketplaceQuery : IRequest<EstadisticasMarketplaceDto>;

public record ConteoDto(string Etiqueta, int Total);
public record ConteoSemanalDto(DateOnly InicioSemana, int Total);
public record EmbudoPostulacionesDto(int Enviadas, int EnRevision, int Aceptadas, int NoSeleccionadas);

public record EstadisticasMarketplaceDto(
    int OfertasDisponibles,
    IReadOnlyList<ConteoDto> OfertasPorDisciplina,
    IReadOnlyList<ConteoSemanalDto> OfertasPorSemana,
    EmbudoPostulacionesDto Postulaciones);
