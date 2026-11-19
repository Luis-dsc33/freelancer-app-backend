namespace Marketplace.Domain.Entities;

public class Entregable
{
    public Guid Id { get; set; }
    public Guid ProyectoId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public int Orden { get; set; }
    public Proyecto Proyecto { get; set; } = default!;
}
