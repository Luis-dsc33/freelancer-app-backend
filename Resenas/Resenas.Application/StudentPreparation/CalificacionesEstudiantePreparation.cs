namespace Resenas.Application.StudentPreparation;

public static class CalificacionesEstudiantePreparation
{
    // ERJ - 15/11/2026: Implementar POST /api/resenas/contratos/{contratoId}/cliente para que el rol Estudiante califique al Cliente después de un contrato Finalizado.
    // ERJ - 15/11/2026: El autor debe obtenerse del claim validado y el destinatario debe obtenerse del contrato verificado por el servicio Marketplace; nunca aceptar AutorId o DestinatarioId desde el cuerpo.
    // ERJ - 15/11/2026: Rechazar contratos inexistentes, no finalizados, no pertenecientes al Estudiante autenticado y reseñas duplicadas por ContratoId + AutorId + DestinatarioId.
    // ERJ - 15/11/2026: Reutilizar Resena, los DTOs y repositorios que se definan para la calificación del Cliente al Estudiante.
    public const string ContratoPendienteDeImplementacion = "Calificación del Cliente por el Estudiante pendiente de implementación";
}
