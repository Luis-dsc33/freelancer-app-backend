namespace Usuarios.Domain.Entities;

public class TrabajoPortafolio
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = default!;
    public string Titulo { get; set; } = default!;
    public string Descripcion { get; set; } = default!;
    public string? Enlace { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? ActualizadoEn { get; set; }
}
