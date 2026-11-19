using Marketplace.Domain.Entities;
using Marketplace.Domain.Enums;

namespace Marketplace.Application.Queries.ListarOportunidades;

public static class ProyectoMapper
{
    public static OportunidadProyectoDto ToDto(Proyecto proyecto) => ToDto(proyecto, yaPostulado: false);

    public static OportunidadProyectoDto ToDto(Proyecto proyecto, bool yaPostulado)
    {
        var skillsCatalogo = (proyecto.Skills ?? [])
            .Select(x => new ProyectoSkillDto(
                x.SkillId,
                x.Skill?.Nombre ?? string.Empty,
                x.TipoRequisito.ToString()))
            .ToList();

        var requeridasDesdeCatalogo = (proyecto.Skills ?? [])
            .Where(x => x.TipoRequisito == TipoRequisitoSkill.Requerida && !string.IsNullOrWhiteSpace(x.Skill?.Nombre))
            .Select(x => x.Skill.Nombre.Trim());

        var habilidadesRequeridas = (proyecto.HabilidadesRequeridas ?? [])
            .Concat(requeridasDesdeCatalogo)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var habilidadesDeseables = (proyecto.Skills ?? [])
            .Where(x => x.TipoRequisito == TipoRequisitoSkill.Deseable && !string.IsNullOrWhiteSpace(x.Skill?.Nombre))
            .Select(x => x.Skill.Nombre.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        return new OportunidadProyectoDto(
            proyecto.Id,
            proyecto.Titulo,
            proyecto.Descripcion,
            proyecto.Disciplina,
            proyecto.Presupuesto,
            proyecto.FechaLimite,
            proyecto.EsUrgente,
            habilidadesRequeridas,
            proyecto.NombreCliente,
            proyecto.Estado,
            DateTime.SpecifyKind(proyecto.CreadoEn, DateTimeKind.Utc))
        {
            ClienteId = proyecto.ClienteId,
            Alcance = proyecto.Alcance,
            FechaEntregaEstimada = proyecto.FechaEntregaEstimada,
            Modalidad = proyecto.Modalidad.ToString(),
            Ubicacion = proyecto.Ubicacion,
            PresupuestoMinimo = proyecto.PresupuestoMinimo,
            PresupuestoMaximo = proyecto.PresupuestoMaximo,
            TipoPresupuesto = proyecto.TipoPresupuesto.ToString(),
            Moneda = string.IsNullOrWhiteSpace(proyecto.Moneda) ? "MXN" : proyecto.Moneda,
            EsquemaPago = proyecto.EsquemaPago.ToString(),
            HitosPago = (proyecto.HitosPago ?? [])
                .OrderBy(h => h.Orden)
                .Select(h => new HitoPagoProyectoDto(
                    string.IsNullOrWhiteSpace(h.Descripcion) ? $"Hito {h.Orden + 1}" : h.Descripcion,
                    h.Porcentaje ?? 0m,
                    h.Importe,
                    null))
                .ToList(),
            CantidadVacantes = proyecto.CantidadVacantes,
            Entregables = (proyecto.Entregables ?? []).OrderBy(x => x.Orden).Select(x => x.Descripcion).ToList(),
            Habilidades = (proyecto.Skills ?? []).Select(x => new SkillRequirementDto(
                x.SkillId,
                x.Skill?.Nombre ?? string.Empty,
                x.Skill?.Categoria?.Nombre ?? string.Empty,
                x.TipoRequisito.ToString())).ToList(),
            HabilidadesDeseables = habilidadesDeseables,
            Skills = skillsCatalogo,
            NivelAvance = proyecto.NivelAvance?.ToString(),
            NivelAcademicoSugerido = proyecto.NivelAcademicoSugerido,
            RequierePortafolio = proyecto.RequierePortafolio,
            DedicacionSemanalEstimada = proyecto.DedicacionSemanalEstimada,
            FlexibilidadHorario = proyecto.FlexibilidadHorario,
            PermitePortafolioPublico = proyecto.PermitePortafolioPublico,
            RequiereConfidencialidad = proyecto.RequiereConfidencialidad,
            PropiedadIntelectual = proyecto.PropiedadIntelectual?.ToString(),
            YaPostulado = yaPostulado
        };
    }
}
