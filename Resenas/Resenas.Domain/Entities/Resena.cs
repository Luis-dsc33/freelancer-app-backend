namespace Resenas.Domain.Entities;

public class Resena
{
    public Guid Id { get; set; }
    public Guid ContratoId { get; set; }
    public Guid AutorId { get; set; }
    public Guid DestinatarioId { get; set; }
    public int Puntuacion { get; set; }
    public string Comentario { get; set; } = string.Empty;
    public DateTime CreadaEn { get; set; }
}
