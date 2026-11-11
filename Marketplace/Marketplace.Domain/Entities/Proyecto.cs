namespace Marketplace.Domain.Entities;

/// <summary>
/// Proyecto / oportunidad publicada en el marketplace por un cliente.
/// </summary>
public class Proyecto
{
    public const string EstadoAbierto = "Abierto";

    public const decimal PresupuestoMinimo = 100m;
    public const decimal PresupuestoMaximo = 500000m;

    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>Presupuesto en MXN (rango de negocio: 100 a 500000).</summary>
    public decimal Presupuesto { get; set; }

    public bool EsUrgente { get; set; }
    public List<string> HabilidadesRequeridas { get; set; } = new();

    /// <summary>Nombre visible del cliente que publicó el proyecto.</summary>
    public string NombreCliente { get; set; } = string.Empty;

    public string Estado { get; set; } = EstadoAbierto;
    public DateTime CreadoEn { get; set; }
}

