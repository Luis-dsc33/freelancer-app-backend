using Marketplace.Application.Commands.CrearOferta;
using Marketplace.Domain.Enums;
using MediatR;
using Marketplace.Application.Queries.ListarOportunidades;

namespace Marketplace.Application.Commands.ActualizarOferta;

public sealed record ActualizarOfertaCommand(
    Guid Id,
    string Titulo,
    string Descripcion,
    string Disciplina,
    decimal? Presupuesto,
    decimal? PresupuestoMinimo,
    decimal? PresupuestoMaximo,
    TipoPresupuesto TipoPresupuesto,
    string Moneda,
    DateOnly FechaLimite,
    DateOnly? FechaEntregaEstimada,
    ModalidadProyecto Modalidad,
    string? Ubicacion,
    string? Alcance,
    List<Guid> HabilidadesRequeridas,
    List<Guid> HabilidadesDeseables,
    List<string> Entregables,
    NivelAvanceProyecto? NivelAvance,
    string? NivelAcademicoSugerido,
    bool RequierePortafolio,
    string? DedicacionSemanalEstimada,
    string? FlexibilidadHorario,
    EsquemaPago EsquemaPago,
    List<HitoPagoInput> HitosPago,
    string? GastosAdicionales,
    int CantidadVacantes,
    bool PermitePortafolioPublico,
    bool RequiereConfidencialidad,
    PropiedadIntelectual? PropiedadIntelectual) : IRequest<OportunidadProyectoDto>;
