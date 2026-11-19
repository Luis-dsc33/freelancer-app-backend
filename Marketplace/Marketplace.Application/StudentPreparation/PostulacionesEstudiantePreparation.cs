namespace Marketplace.Application.StudentPreparation;

public static class PostulacionesEstudiantePreparation
{
    // ERJ - 15/11/2026: Consumir GET /api/catalogo/habilidades y persistir únicamente SkillId en el perfil del Estudiante.
    // ERJ - 15/11/2026: No crear FK ni navegación EF entre Usuarios.PerfilEstudiante y Marketplace.Skill; validar los ids mediante el contrato del catálogo.
    // ERJ - 15/11/2026: La relación EstudianteSkill deberá vivir en el servicio propietario del perfil del Estudiante e incluir NivelDominio declarado por el estudiante.
    // ERJ - 15/11/2026: El matching debe comparar EstudianteSkill con ProyectoSkill por SkillId, separando TipoRequisito.Requerida de TipoRequisito.Deseable.
    // ERJ - 15/11/2026: Implementar POST /api/ofertas/{ofertaId}/postulaciones para el rol Estudiante.
    // ERJ - 15/11/2026: El handler debe obtener EstudianteId exclusivamente del claim validado, verificar que la oferta esté Abierta y no haya vencido, y rechazar duplicados mediante ExisteParaEstudianteAsync y la restricción única de base de datos.
    // ERJ - 15/11/2026: El cuerpo debe aceptar únicamente el mensaje de postulación; no debe permitir asignar EstudianteId, estado, fechas ni ProyectoId.
    // ERJ - 15/11/2026: La respuesta esperada es 201 Created con PostulacionDto y los errores de oferta inexistente, cerrada, vencida o postulación duplicada deben traducirse a respuestas HTTP apropiadas.
    // ERJ - 15/11/2026: Implementar GET /api/postulaciones/mis-postulaciones para devolver solo las postulaciones del estudiante autenticado, sin exponer datos privados del Cliente.
    // ERJ - 15/11/2026: Reutilizar Postulacion, PostulacionDto, IPostulacionRepository y los estados Pendiente, Aceptada y Rechazada definidos en Marketplace.Domain.
    public const string ContratoPendienteDeImplementacion = "Postulaciones del Estudiante pendientes de implementación";
}
