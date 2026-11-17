namespace Marketplace.Domain.Entities;

public class Contrato
{
    public const string EstadoActivo = "Activo";
    public const string EstadoFinalizado = "Finalizado";
    public const string EstadoCancelado = "Cancelado";

    public Guid Id { get; set; }
    public Guid PostulacionId { get; set; }
    public Guid ProyectoId { get; set; }
    public Guid ClienteId { get; set; }
    public Guid EstudianteId { get; set; }
    public decimal ImporteAcordado { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly FechaCompromiso { get; set; }
    public string Estado { get; set; } = EstadoActivo;
    public DateTime CreadoEn { get; set; }
    public DateTime? CerradoEn { get; set; }
    public Postulacion Postulacion { get; set; } = default!;
}
