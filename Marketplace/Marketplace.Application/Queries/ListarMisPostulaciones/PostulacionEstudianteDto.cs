namespace Marketplace.Application.Queries.ListarMisPostulaciones;

public sealed record PostulacionEstudianteDto(
    Guid Id,
    Guid ProyectoId,
    Guid EstudianteId,
    string Mensaje,
    string Estado,
    DateTime CreadaEn,
    DateTime? ResueltaEn,
    string TituloProyecto,
    string DisciplinaProyecto,
    string ModalidadProyecto,
    decimal PresupuestoProyecto,
    string NombreCliente,
    string EstadoProyecto,
    Guid? ContratoId);
