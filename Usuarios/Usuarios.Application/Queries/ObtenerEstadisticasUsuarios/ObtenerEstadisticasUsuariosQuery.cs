// ADMIN Marcos: HU-28 estadísticas que consume el panel del servicio Admin.
// La forma del JSON coincide con EstadisticasUsuariosDto de Admin.Application.
using MediatR;

namespace Usuarios.Application.Queries.ObtenerEstadisticasUsuarios;

public record ObtenerEstadisticasUsuariosQuery : IRequest<EstadisticasUsuariosDto>;

public record ConteoSemanalDto(DateOnly InicioSemana, int Total);

public record EstadisticasUsuariosDto(
    int TotalEstudiantes, int TotalClientes, int CuentasSuspendidas, IReadOnlyList<ConteoSemanalDto> RegistrosPorSemana);
