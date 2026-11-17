using Marketplace.Application.Commands.CrearOferta;
using Marketplace.Domain.Enums;

namespace Marketplace.Api.Requests.Proyectos;

public sealed record CrearOfertaRequest(
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
    List<Guid>? HabilidadesRequeridas,
    List<Guid>? HabilidadesDeseables,
    List<string>? Entregables,
    NivelAvanceProyecto? NivelAvance,
    string? NivelAcademicoSugerido,
    bool RequierePortafolio,
    string? DedicacionSemanalEstimada,
    string? FlexibilidadHorario,
    EsquemaPago EsquemaPago,
    List<HitoPagoInput>? HitosPago,
    string? GastosAdicionales,
    int CantidadVacantes = 1,
    bool PermitePortafolioPublico = false,
    bool RequiereConfidencialidad = false,
    PropiedadIntelectual? PropiedadIntelectual = null);
