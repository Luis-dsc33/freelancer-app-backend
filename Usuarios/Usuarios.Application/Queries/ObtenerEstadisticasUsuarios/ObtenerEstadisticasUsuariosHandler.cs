// ADMIN Marcos: HU-28
using MediatR;
using Usuarios.Application.Commands.CambiarEstadoUsuario;
using Usuarios.Application.Interfaces;

namespace Usuarios.Application.Queries.ObtenerEstadisticasUsuarios;

public class ObtenerEstadisticasUsuariosHandler(IUsuarioAdminRepository repositorio)
    : IRequestHandler<ObtenerEstadisticasUsuariosQuery, EstadisticasUsuariosDto>
{
    private const int Semanas = 8;

    public async Task<EstadisticasUsuariosDto> Handle(ObtenerEstadisticasUsuariosQuery request, CancellationToken ct)
    {
        var porRol = await repositorio.ContarPorRolAsync(ct);
        var suspendidas = await repositorio.ContarPorEstadoAsync(CambiarEstadoUsuarioHandler.EstadoSuspendido, ct);

        // Semanas de lunes a domingo; el filtro de fechas se hace en SQL y solo se agrupa lo de la ventana
        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);
        var lunesActual = hoy.AddDays(-(((int)hoy.DayOfWeek + 6) % 7));
        var inicio = lunesActual.AddDays(-7 * (Semanas - 1));
        var fechas = await repositorio.FechasDeRegistroDesdeAsync(inicio.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc), ct);

        var porSemana = Enumerable.Range(0, Semanas)
            .Select(i => inicio.AddDays(7 * i))
            .Select(semana => new ConteoSemanalDto(semana, fechas.Count(f =>
            {
                var dia = DateOnly.FromDateTime(f);
                return dia >= semana && dia < semana.AddDays(7);
            })))
            .ToList();

        return new EstadisticasUsuariosDto(
            porRol.GetValueOrDefault("Estudiante"),
            porRol.GetValueOrDefault("Cliente"),
            suspendidas,
            porSemana);
    }
}
