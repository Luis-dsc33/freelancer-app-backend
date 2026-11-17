namespace Marketplace.Domain.Entities;

public class Postulacion
{
    public const string EstadoPendiente = "Pendiente";
    public const string EstadoAceptada = "Aceptada";
    public const string EstadoRechazada = "Rechazada";

    public Guid Id { get; set; }
    public Guid ProyectoId { get; set; }
    public Guid EstudianteId { get; set; }
    public string Estado { get; set; } = EstadoPendiente;
    public string Mensaje { get; set; } = string.Empty;
    public DateTime CreadaEn { get; set; }
    public DateTime? ResueltaEn { get; set; }
    public Proyecto Proyecto { get; set; } = default!;
}
