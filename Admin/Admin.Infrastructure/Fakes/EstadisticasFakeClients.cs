using Admin.Application.Dtos;
using Admin.Application.Interfaces;

namespace Admin.Infrastructure.Fakes;

// DATOS DE DEMOSTRACIÓN — solo se registran cuando Metricas:UsarDatosDemostracion = true.
// Sirven mientras los demás servicios publican su endpoint de estadísticas.
// Para usar datos reales basta con poner la bandera en false (ver Program.cs).

internal static class Semanas
{
    // Últimas N semanas, cada una iniciando en lunes, de la más antigua a la más reciente.
    public static IReadOnlyList<ConteoSemanalDto> Ultimas(params int[] totales)
    {
        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);
        var lunesActual = hoy.AddDays(-(((int)hoy.DayOfWeek + 6) % 7));
        return totales
            .Select((total, i) => new ConteoSemanalDto(lunesActual.AddDays(-7 * (totales.Length - 1 - i)), total))
            .ToList();
    }
}

public class UsuariosFakeClient : IUsuariosClient
{
    public Task<EstadisticasUsuariosDto> ObtenerEstadisticasAsync(CancellationToken ct) =>
        Task.FromResult(new EstadisticasUsuariosDto(
            TotalEstudiantes: 128,
            TotalClientes: 47,
            CuentasSuspendidas: 3,
            RegistrosPorSemana: Semanas.Ultimas(9, 12, 15, 11, 18, 22, 19, 26)));
}

public class MarketplaceFakeClient : IMarketplaceClient
{
    public Task<EstadisticasMarketplaceDto> ObtenerEstadisticasAsync(CancellationToken ct) =>
        Task.FromResult(new EstadisticasMarketplaceDto(
            OfertasDisponibles: 34,
            OfertasPorDisciplina:
            [
                new("Desarrollo web", 14),
                new("Diseño gráfico", 9),
                new("Marketing digital", 6),
                new("Ciencia de datos", 5),
                new("Traducción", 3),
                new("Video y animación", 2)
            ],
            OfertasPorSemana: Semanas.Ultimas(3, 4, 6, 5, 7, 9, 8, 11),
            Postulaciones: new EmbudoPostulacionesDto(Enviadas: 41, EnRevision: 27, Aceptadas: 18, NoSeleccionadas: 23)));
}

public class ResenasFakeClient : IResenasClient
{
    public Task<EstadisticasResenasDto> ObtenerEstadisticasAsync(CancellationToken ct) =>
        Task.FromResult(new EstadisticasResenasDto(
            TotalCalificaciones: 62,
            PromedioGeneral: 4.2,
            DistribucionEstrellas: [new("1", 2), new("2", 3), new("3", 7), new("4", 19), new("5", 31)]));
}

public class CasosFakeClient : ICasosClient
{
    public Task<EstadisticasCasosDto> ObtenerEstadisticasAsync(CancellationToken ct) =>
        Task.FromResult(new EstadisticasCasosDto(
            Recibidos: 2, EnRevision: 3, PendientesInformacion: 1, Cerrados: 9, DiasPromedioCierre: 3.4));
}
