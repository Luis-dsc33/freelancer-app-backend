namespace Marketplace.Domain.Entities;

public class HitoPago
{
    public Guid Id { get; set; }
    public Guid ProyectoId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal? Porcentaje { get; set; }
    public decimal? Importe { get; set; }
    public int Orden { get; set; }
    public Proyecto Proyecto { get; set; } = default!;
}
