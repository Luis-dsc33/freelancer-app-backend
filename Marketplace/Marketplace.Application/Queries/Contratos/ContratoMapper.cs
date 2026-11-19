using Marketplace.Domain.Entities;

namespace Marketplace.Application.Queries.Contratos;

public static class ContratoMapper
{
    public static ContratoDto ToDto(Contrato contrato) => new(
        contrato.Id,
        contrato.PostulacionId,
        contrato.ProyectoId,
        contrato.ClienteId,
        contrato.EstudianteId,
        contrato.ImporteAcordado,
        contrato.FechaInicio,
        contrato.FechaCompromiso,
        contrato.Estado,
        DateTime.SpecifyKind(contrato.CreadoEn, DateTimeKind.Utc),
        contrato.CerradoEn.HasValue ? DateTime.SpecifyKind(contrato.CerradoEn.Value, DateTimeKind.Utc) : null)
    {
        TituloProyecto = contrato.Postulacion?.Proyecto?.Titulo,
        NombreCliente = contrato.Postulacion?.Proyecto?.NombreCliente
    };
}
