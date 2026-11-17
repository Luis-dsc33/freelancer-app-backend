namespace Usuarios.Application.Queries.ObtenerPerfilCliente;

public sealed class PerfilClienteDto
{
    public Guid Id { get; init; }
    public Guid UsuarioId { get; init; }
    public string NombreEmpresa { get; init; } = string.Empty;
    public string Telefono { get; init; } = string.Empty;
    public string Ubicacion { get; init; } = string.Empty;
    public string Descripcion { get; init; } = string.Empty;
    public string DatosContacto { get; init; } = string.Empty;
}
