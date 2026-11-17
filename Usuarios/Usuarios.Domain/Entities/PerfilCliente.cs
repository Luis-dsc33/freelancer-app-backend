namespace Usuarios.Domain.Entities;

public class PerfilCliente
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = default!;
    public string NombreEmpresa { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Ubicacion { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string DatosContacto { get; set; } = string.Empty;
}
