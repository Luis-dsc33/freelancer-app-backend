// ADMIN Marcos: HU-27 registro inalterable de cada medida administrativa (quién, qué, sobre qué, cuándo y por qué).
namespace Admin.Domain.Entities;

public class AccionAdministrativa
{
    public const string Suspender = "Suspender";
    public const string Reactivar = "Reactivar";
    public const string Ocultar = "Ocultar";
    public const string Restaurar = "Restaurar";

    public const string RecursoUsuario = "Usuario";
    public const string RecursoOferta = "Oferta";

    public Guid Id { get; set; }
    public Guid AdminId { get; set; }
    public string AdminNombre { get; set; } = string.Empty;
    public string TipoAccion { get; set; } = string.Empty;   // texto: agregar una acción nueva no requiere migración
    public string RecursoTipo { get; set; } = string.Empty;
    public Guid RecursoId { get; set; }
    public string RecursoNombre { get; set; } = string.Empty; // copia legible al momento de la acción
    public string Motivo { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
}
