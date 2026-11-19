namespace Usuarios.Application.StudentPreparation;

public static class PerfilYPortafolioEstudiantePreparation
{
    // ERJ - 15/11/2026: Mantener GET, POST y PUT /api/Perfiles/mi-perfil exclusivamente para Estudiante; el handler debe usar ICurrentUserService y nunca aceptar UsuarioId en el cuerpo.
    // ERJ - 15/11/2026: El perfil debe validar Carrera, Habilidades y Descripcion, normalizar habilidades y conservar la unicidad por usuario mediante IPerfilEstudianteRepository.
    // ERJ - 15/11/2026: Mantener GET /api/Portafolios/mi-portafolio, POST /api/Portafolios/trabajos, PUT /api/Portafolios/trabajos/{id} y DELETE /api/Portafolios/trabajos/{id}; toda modificación debe comprobar UsuarioId mediante el claim.
    // ERJ - 15/11/2026: Mantener GET público /api/Portafolios/estudiantes/{usuarioId} solo lectura, responder con DTO público específico y no exponer correo, teléfono, roles, hashes, tokens ni entidades de Identity.
    // ERJ - 15/11/2026: Para integrar con Marketplace, el DTO público debe conservar UsuarioId, carrera, habilidades, descripción y trabajos públicos, cualquier dato nuevo debe documentarse antes de exponerlo.
    public const string ContratoPendienteDeImplementacion = "Lógica funcional de Perfil y Portafolio del Estudiante";
}
