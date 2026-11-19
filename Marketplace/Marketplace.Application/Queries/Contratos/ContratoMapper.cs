using Marketplace.Domain.Entities;

namespace Marketplace.Application.Queries.Contratos;

public static class ContratoMapper
{
    public static ContratoDto ToDto(Contrato contrato) => new(
        contrato.Id, contrato.PostulacionId, contrato.ProyectoId, contrato.ClienteId,
        contrato.EstudianteId, contrato.ImporteAcordado, contrato.FechaInicio,
        contrato.FechaCompromiso, contrato.Estado, contrato.CreadoEn, contrato.CerradoEn);
}
