namespace Usuarios.Domain.Entities;

public class Usuario
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public Guid RolId { get; set; }
    public Rol Rol { get; set; } = default!; 
    public string Estado { get; set; } = "Activo";
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // ADMIN Marcos: motivo de la suspensión vigente (HU-21). Se muestra al usuario si intenta iniciar sesión.
    // Se limpia al reactivar la cuenta (HU-26). El historial completo vive en la bitácora del servicio Admin.
    public string? MotivoSuspension { get; set; }
}