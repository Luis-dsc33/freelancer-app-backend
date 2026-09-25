namespace Usuarios.Application.Interfaces;

public interface IEmailSender
{
    Task EnviarEnlaceRestablecimientoAsync(
        string destinatario,
        string nombre,
        string enlace,
        int vigenciaMinutos,
        CancellationToken cancellationToken);
}
