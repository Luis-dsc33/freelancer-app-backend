using MediatR;
using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.ListarOportunidades;

namespace Marketplace.Application.Commands.ActualizarOferta;

public sealed class ActualizarOfertaHandler(
    IProyectoRepository proyectos,
    ISkillCatalogRepository skills,
    ICurrentUserService currentUser)
    : IRequestHandler<ActualizarOfertaCommand, OportunidadProyectoDto>
{
    public async Task<OportunidadProyectoDto> Handle(ActualizarOfertaCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al cliente autenticado.");

        var proyecto = await proyectos.ObtenerPorIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("La oferta no existe.");
        if (proyecto.ClienteId != currentUser.UserId)
            throw new UnauthorizedAccessException("La oferta no pertenece al cliente autenticado.");
        if (proyecto.Estado is Domain.Entities.Proyecto.EstadoCerrado or Domain.Entities.Proyecto.EstadoCancelado)
            throw new InvalidOperationException("Una oferta cerrada no puede modificarse.");

        var skillIds = request.HabilidadesRequeridas.Concat(request.HabilidadesDeseables).Distinct().ToArray();
        if (!await skills.ExistenActivasAsync(skillIds, cancellationToken))
            throw new InvalidOperationException("Una o más habilidades no existen o están inactivas.");

        proyecto.Titulo = request.Titulo.Trim();
        proyecto.Descripcion = request.Descripcion.Trim();
        proyecto.Disciplina = request.Disciplina.Trim();
        proyecto.Presupuesto = request.Presupuesto ?? 0m;
        proyecto.TipoPresupuesto = request.TipoPresupuesto;
        proyecto.PresupuestoMinimo = request.PresupuestoMinimo;
        proyecto.PresupuestoMaximo = request.PresupuestoMaximo;
        proyecto.Moneda = request.Moneda.Trim().ToUpperInvariant();
        proyecto.FechaEntregaEstimada = request.FechaEntregaEstimada;
        proyecto.FechaLimite = request.FechaLimite;
        proyecto.Modalidad = request.Modalidad;
        proyecto.Ubicacion = request.Ubicacion?.Trim();
        proyecto.Alcance = request.Alcance?.Trim();
        proyecto.NivelAvance = request.NivelAvance;
        proyecto.NivelAcademicoSugerido = request.NivelAcademicoSugerido?.Trim();
        proyecto.RequierePortafolio = request.RequierePortafolio;
        proyecto.DedicacionSemanalEstimada = request.DedicacionSemanalEstimada?.Trim();
        proyecto.FlexibilidadHorario = request.FlexibilidadHorario?.Trim();
        proyecto.EsquemaPago = request.EsquemaPago;
        proyecto.GastosAdicionales = request.GastosAdicionales?.Trim();
        proyecto.CantidadVacantes = request.CantidadVacantes;
        proyecto.PermitePortafolioPublico = request.PermitePortafolioPublico;
        proyecto.RequiereConfidencialidad = request.RequiereConfidencialidad;
        proyecto.PropiedadIntelectual = request.PropiedadIntelectual;
        proyecto.Skills.Clear();
        proyecto.Skills.AddRange(request.HabilidadesRequeridas.Select(id => new Domain.Entities.ProyectoSkill { ProyectoId = proyecto.Id, SkillId = id, TipoRequisito = Domain.Enums.TipoRequisitoSkill.Requerida }));
        proyecto.Skills.AddRange(request.HabilidadesDeseables.Select(id => new Domain.Entities.ProyectoSkill { ProyectoId = proyecto.Id, SkillId = id, TipoRequisito = Domain.Enums.TipoRequisitoSkill.Deseable }));
        proyecto.Entregables.Clear();
        proyecto.Entregables.AddRange(request.Entregables.Select((description, index) => new Domain.Entities.Entregable { Id = Guid.NewGuid(), ProyectoId = proyecto.Id, Descripcion = description.Trim(), Orden = index }));
        proyecto.HitosPago.Clear();
        proyecto.HitosPago.AddRange(request.HitosPago.Select(x => new Domain.Entities.HitoPago { Id = Guid.NewGuid(), ProyectoId = proyecto.Id, Descripcion = x.Descripcion.Trim(), Porcentaje = x.Porcentaje, Importe = x.Importe, Orden = x.Orden }));
        proyecto.ActualizadoEn = DateTime.UtcNow;
        await proyectos.ActualizarAsync(proyecto, cancellationToken);
        return ProyectoMapper.ToDto(proyecto);
    }
}
