using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Usuarios.Application.Exceptions;
using Usuarios.Application.Interfaces;

namespace Usuarios.Infrastructure.Security;

public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public SmtpEmailSender(IConfiguration configuration) => _configuration = configuration;

    public async Task EnviarEnlaceRestablecimientoAsync(
        string destinatario,
        string nombre,
        string enlace,
        int vigenciaMinutos,
        CancellationToken cancellationToken)
    {
        var host = _configuration["Smtp:Host"];
        var remitente = _configuration["Smtp:FromEmail"];
        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(remitente))
            throw new EmailDeliveryException("El servicio de correo no está configurado.");

        var port = int.TryParse(_configuration["Smtp:Port"], out var configuredPort)
            ? configuredPort
            : 587;
        var nombreRemitente = _configuration["Smtp:FromName"] ?? "CodeBridge";
        var username = _configuration["Smtp:Username"];
        var password = _configuration["Smtp:Password"];

        var mensaje = new MimeMessage();
        mensaje.From.Add(new MailboxAddress(nombreRemitente, remitente));
        mensaje.To.Add(MailboxAddress.Parse(destinatario));
        mensaje.Subject = "Restablece tu contraseña - CodeBridge";
        mensaje.Body = new TextPart("plain")
        {
            Text = $"Hola {nombre},\n\n" +
                   "Recibimos una solicitud para restablecer la contraseña de tu cuenta. " +
                   "Usa este enlace para elegir una nueva contraseña:\n\n" +
                   $"{enlace}\n\n" +
                   $"El enlace vence en {vigenciaMinutos} minutos.\n\n" +
                   "Si no solicitaste este cambio, puedes ignorar este correo."
        };

        try
        {
            using var cliente = new SmtpClient();
            await cliente.ConnectAsync(host, port, SecureSocketOptions.Auto, cancellationToken);

            if (!string.IsNullOrWhiteSpace(username))
                await cliente.AuthenticateAsync(username, password ?? string.Empty, cancellationToken);

            await cliente.SendAsync(mensaje, cancellationToken);
            await cliente.DisconnectAsync(true, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new EmailDeliveryException("No fue posible enviar el correo de recuperación.", ex);
        }
    }
}
