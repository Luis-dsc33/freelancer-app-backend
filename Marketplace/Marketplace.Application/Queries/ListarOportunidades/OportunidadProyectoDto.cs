namespace Marketplace.Application.Queries.ListarOportunidades;

/// <summary>
/// Contrato que consume el frontend (modelo OportunidadProyecto). Se serializa en camelCase.
/// </summary>
public sealed record OportunidadProyectoDto(
    Guid Id,
    string Titulo,
    string Descripcion,
    string Disciplina,
    decimal Presupuesto,
    DateOnly FechaLimite,
    bool EsUrgente,
    IReadOnlyList<string> HabilidadesRequeridas,
    string NombreCliente,
    string Estado,
    DateTime CreadoEn)
{
    public Guid ClienteId { get; init; }
    public string? Alcance { get; init; }
    public DateOnly? FechaEntregaEstimada { get; init; }
    public string? Modalidad { get; init; }
    public string? Ubicacion { get; init; }
    public decimal? PresupuestoMinimo { get; init; }
    public decimal? PresupuestoMaximo { get; init; }
    public string? TipoPresupuesto { get; init; }
    public string Moneda { get; init; } = "MXN";
    public string? EsquemaPago { get; init; }
    public IReadOnlyList<HitoPagoProyectoDto> HitosPago { get; init; } = [];
    public int CantidadVacantes { get; init; } = 1;
    public IReadOnlyList<SkillRequirementDto> Habilidades { get; init; } = [];
    public IReadOnlyList<string> HabilidadesDeseables { get; init; } = [];
    public IReadOnlyList<ProyectoSkillDto> Skills { get; init; } = [];
    public IReadOnlyList<string> Entregables { get; init; } = [];
    public string? NivelAvance { get; init; }
    public string? NivelAcademicoSugerido { get; init; }
    public bool RequierePortafolio { get; init; }
    public string? DedicacionSemanalEstimada { get; init; }
    public string? FlexibilidadHorario { get; init; }
    public bool PermitePortafolioPublico { get; init; }
    public bool RequiereConfidencialidad { get; init; }
    public string? PropiedadIntelectual { get; init; }
    public bool YaPostulado { get; init; }
}

public sealed record SkillRequirementDto(Guid Id, string Nombre, string Categoria, string TipoRequisito);

public sealed record ProyectoSkillDto(Guid SkillId, string NombreSkill, string TipoRequisito);

public sealed record HitoPagoProyectoDto(string Titulo, decimal Porcentaje, decimal? Monto, string? Descripcion);
