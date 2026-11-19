namespace Marketplace.Domain.Entities;

using Marketplace.Domain.Enums;

/// <summary>
/// Proyecto / oportunidad publicada en el marketplace por un cliente.
/// </summary>
public class Proyecto
{
    public const string EstadoBorrador = "Borrador";
    public const string EstadoAbierto = "Abierto";
    /// <summary>Oferta ocultada por un administrador (moderación). Solo el admin puede restaurarla.</summary>
    public const string EstadoOculta = "Oculta";
    public const string EstadoPublicada = "Publicada";
    public const string EstadoEnProceso = "EnProceso";
    public const string EstadoCerrado = "Cerrado";
    public const string EstadoCancelado = "Cancelada";

    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Disciplina { get; set; } = string.Empty;

    /// <summary>Presupuesto en MXN.</summary>
    public decimal Presupuesto { get; set; }
    public TipoPresupuesto TipoPresupuesto { get; set; } = TipoPresupuesto.Fijo;
    public decimal? PresupuestoMinimo { get; set; }
    public decimal? PresupuestoMaximo { get; set; }
    public string Moneda { get; set; } = "MXN";
    public EsquemaPago EsquemaPago { get; set; } = EsquemaPago.Unico;

    public DateOnly FechaLimite { get; set; }
    public DateOnly? FechaEntregaEstimada { get; set; }
    public Guid ClienteId { get; set; }

    public bool EsUrgente { get; set; }
    public List<string> HabilidadesRequeridas { get; set; } = new();
    public ModalidadProyecto Modalidad { get; set; } = ModalidadProyecto.Remoto;
    public string? Ubicacion { get; set; }
    public string? FlexibilidadHorario { get; set; }
    public string? DedicacionSemanalEstimada { get; set; }
    public string? NivelAcademicoSugerido { get; set; }
    public NivelAvanceProyecto? NivelAvance { get; set; }
    public bool RequierePortafolio { get; set; }
    public int CantidadVacantes { get; set; } = 1;
    public bool PermitePortafolioPublico { get; set; }
    public bool RequiereConfidencialidad { get; set; }
    public PropiedadIntelectual? PropiedadIntelectual { get; set; }
    public string? GastosAdicionales { get; set; }
    public string? Alcance { get; set; }

    /// <summary>Nombre visible del cliente que publicó el proyecto.</summary>
    public string NombreCliente { get; set; } = string.Empty;

    public string Estado { get; set; } = EstadoAbierto;
    public DateTime CreadoEn { get; set; }
    public DateTime? ActualizadoEn { get; set; }
    public DateTime? PublicadoEn { get; set; }
    public DateTime? CerradoEn { get; set; }

    public List<ProyectoSkill> Skills { get; set; } = new();
    public List<Entregable> Entregables { get; set; } = new();
    public List<HitoPago> HitosPago { get; set; } = new();
}
