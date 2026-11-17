namespace Marketplace.Application.StudentPreparation;

public static class ContratosEstudiantePreparation
{
    // ERJ - 15/11/2026: Implementar GET /api/contratos/mis-contratos para el rol Estudiante filtrando exclusivamente por el EstudianteId del claim.
    // ERJ - 15/11/2026: La respuesta debe reutilizar ContratoDto y no exponer datos privados que no sean necesarios para el participante.
    // ERJ - 15/11/2026: Implementar PUT /api/contratos/{id}/finalizar y PUT /api/contratos/{id}/cancelar para Estudiante solo cuando el contrato pertenezca al claim y el estado actual sea Activo.
    // ERJ - 15/11/2026: No permitir que el cuerpo asigne ClienteId, EstudianteId, ProyectoId, PostulacionId, importe, fechas o estados privilegiados.
    // ERJ - 15/11/2026: Reutilizar Contrato, IContratoRepository, ContratoDto y las transiciones Activo -> Finalizado/Cancelado para mantener compatibilidad con la implementación del Cliente.
    public const string ContratoPendienteDeImplementacion = "Operaciones de contratos del Estudiante pendientes de implementación";
}
