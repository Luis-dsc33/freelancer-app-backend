using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.ListarOportunidades;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Enums;
using MediatR;

namespace Marketplace.Application.Commands.CrearOferta;

public sealed class CrearOfertaHandler(
    IProyectoRepository proyectos,
    ISkillCatalogRepository skills,
    ICurrentUserService currentUser) : IRequestHandler<CrearOfertaCommand, OportunidadProyectoDto>
{
    public async Task<OportunidadProyectoDto> Handle(CrearOfertaCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al cliente autenticado.");

        var skillIds = request.HabilidadesRequeridas.Concat(request.HabilidadesDeseables).Distinct().ToArray();
        if (!await skills.ExistenActivasAsync(skillIds, cancellationToken))
            throw new InvalidOperationException("Una o más habilidades no existen o están inactivas.");

        var proyecto = new Proyecto
        {
            Id = Guid.NewGuid(),
            Titulo = request.Titulo.Trim(),
            Descripcion = request.Descripcion.Trim(),
            Disciplina = request.Disciplina.Trim(),
            Presupuesto = request.Presupuesto ?? 0m,
            TipoPresupuesto = request.TipoPresupuesto,
            PresupuestoMinimo = request.PresupuestoMinimo,
            PresupuestoMaximo = request.PresupuestoMaximo,
            Moneda = request.Moneda.Trim().ToUpperInvariant(),
            EsquemaPago = request.EsquemaPago,
            FechaLimite = request.FechaLimite,
            FechaEntregaEstimada = request.FechaEntregaEstimada,
            ClienteId = currentUser.UserId,
            NombreCliente = "Cliente",
            Modalidad = request.Modalidad,
            Ubicacion = request.Ubicacion?.Trim(),
            Alcance = request.Alcance?.Trim(),
            NivelAvance = request.NivelAvance,
            NivelAcademicoSugerido = request.NivelAcademicoSugerido?.Trim(),
            RequierePortafolio = request.RequierePortafolio,
            DedicacionSemanalEstimada = request.DedicacionSemanalEstimada?.Trim(),
            FlexibilidadHorario = request.FlexibilidadHorario?.Trim(),
            CantidadVacantes = request.CantidadVacantes,
            PermitePortafolioPublico = request.PermitePortafolioPublico,
            RequiereConfidencialidad = request.RequiereConfidencialidad,
            PropiedadIntelectual = request.PropiedadIntelectual,
            GastosAdicionales = request.GastosAdicionales?.Trim(),
            Estado = Proyecto.EstadoBorrador,
            CreadoEn = DateTime.UtcNow
        };

        proyecto.Skills = request.HabilidadesRequeridas.Select(id => new ProyectoSkill { ProyectoId = proyecto.Id, SkillId = id, TipoRequisito = TipoRequisitoSkill.Requerida })
            .Concat(request.HabilidadesDeseables.Select(id => new ProyectoSkill { ProyectoId = proyecto.Id, SkillId = id, TipoRequisito = TipoRequisitoSkill.Deseable }))
            .ToList();
        proyecto.Entregables = request.Entregables.Select((descripcion, index) => new Entregable { Id = Guid.NewGuid(), ProyectoId = proyecto.Id, Descripcion = descripcion.Trim(), Orden = index }).ToList();
        proyecto.HitosPago = request.HitosPago.Select(x => new HitoPago { Id = Guid.NewGuid(), ProyectoId = proyecto.Id, Descripcion = x.Descripcion.Trim(), Porcentaje = x.Porcentaje, Importe = x.Importe, Orden = x.Orden }).ToList();

        await proyectos.AgregarAsync(proyecto, cancellationToken);
        var creada = await proyectos.ObtenerPorIdAsync(proyecto.Id, cancellationToken) ?? proyecto;
        return ProyectoMapper.ToDto(creada);
    }
}
