namespace Usuarios.Domain.Entities;

public class Rol
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = default!; // "Estudiante", "Cliente", "Administrador"
}