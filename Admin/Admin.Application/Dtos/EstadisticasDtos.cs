namespace Admin.Application.Dtos;

// HU-28: contratos de lectura que cada microservicio dueño del dato entrega a Admin.
// Admin NO guarda métricas: solo las pide (HTTP) y las junta para el panel.

public record ConteoDto(string Etiqueta, int Total);

public record ConteoSemanalDto(DateOnly InicioSemana, int Total);

// Servicio dueño: Usuarios
public record EstadisticasUsuariosDto(
    int TotalEstudiantes,
    int TotalClientes,
    int CuentasSuspendidas,
    IReadOnlyList<ConteoSemanalDto> RegistrosPorSemana);

// Servicio dueño: Marketplace
public record EmbudoPostulacionesDto(int Enviadas, int EnRevision, int Aceptadas, int NoSeleccionadas);

public record EstadisticasMarketplaceDto(
    int OfertasDisponibles,
    IReadOnlyList<ConteoDto> OfertasPorDisciplina,
    IReadOnlyList<ConteoSemanalDto> OfertasPorSemana,
    EmbudoPostulacionesDto Postulaciones);

// Servicio dueño: Reseñas
public record EstadisticasResenasDto(
    int TotalCalificaciones,
    double? PromedioGeneral,
    IReadOnlyList<ConteoDto> DistribucionEstrellas);

// Casos de conflicto (HU-23/HU-24). Su ubicación depende del acuerdo A-04.
public record EstadisticasCasosDto(
    int Recibidos,
    int EnRevision,
    int PendientesInformacion,
    int Cerrados,
    double? DiasPromedioCierre);
