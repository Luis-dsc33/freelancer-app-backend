using Marketplace.Domain.Entities;

namespace Marketplace.Application.Queries.ListarOportunidades;

public static class ProyectoMapper
{
    public static OportunidadProyectoDto ToDto(Proyecto proyecto)
    {
        var dto = new OportunidadProyectoDto(
            proyecto.Id,
            proyecto.Titulo,
            proyecto.Descripcion,
            proyecto.Disciplina,
            proyecto.Presupuesto,
            proyecto.FechaLimite,
            proyecto.EsUrgente,
            proyecto.HabilidadesRequeridas,
            proyecto.NombreCliente,
            proyecto.Estado,
            proyecto.CreadoEn)
        {
            Alcance = proyecto.Alcance,
            FechaEntregaEstimada = proyecto.FechaEntregaEstimada,
            Modalidad = proyecto.Modalidad.ToString(),
            Ubicacion = proyecto.Ubicacion,
            PresupuestoMinimo = proyecto.PresupuestoMinimo,
            PresupuestoMaximo = proyecto.PresupuestoMaximo,
            Moneda = proyecto.Moneda,
            EsquemaPago = proyecto.EsquemaPago.ToString(),
            CantidadVacantes = proyecto.CantidadVacantes,
            Entregables = proyecto.Entregables.OrderBy(x => x.Orden).Select(x => x.Descripcion).ToList(),
            Habilidades = proyecto.Skills.Select(x => new SkillRequirementDto(
                x.SkillId,
                x.Skill?.Nombre ?? string.Empty,
                x.Skill?.Categoria?.Nombre ?? string.Empty,
                x.TipoRequisito.ToString())).ToList()
        };
        return dto;
    }
}
