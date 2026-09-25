namespace Usuarios.Domain.Entities;

public class RecuperacionContrasena
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = default!;
    public string TokenHash { get; set; } = default!;
    public DateTime CreadoEn { get; set; }
    public DateTime ExpiraEn { get; set; }
    public DateTime? ConsumidoEn { get; set; }
    public DateTime? RevocadoEn { get; set; }
    public Guid Version { get; set; } = Guid.NewGuid();
}
