using System.Security.Cryptography;
using System.Text;
using MediatR;
using Usuarios.Application.Exceptions;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Application.Commands.SolicitarRecuperacion;

public class SolicitarRecuperacionHandler : IRequestHandler<SolicitarRecuperacionCommand, string>
{
    private const string MensajeConfirmacion =
        "Si el correo está registrado, recibirás un enlace para restablecer tu contraseña.";
    private readonly IUsuarioRepository _usuarioRepositorio;
    private readonly IRecuperacionContrasenaRepository _recuperacionRepositorio;
    private readonly IEmailSender _emailSender;
    private readonly IPasswordResetSettings _configuracion;

    public SolicitarRecuperacionHandler(
        IUsuarioRepository usuarioRepositorio,
        IRecuperacionContrasenaRepository recuperacionRepositorio,
        IEmailSender emailSender,
        IPasswordResetSettings configuracion)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _recuperacionRepositorio = recuperacionRepositorio;
        _emailSender = emailSender;
        _configuracion = configuracion;
    }

    public async Task<string> Handle(SolicitarRecuperacionCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepositorio.ObtenerPorEmailAsync(request.Email.Trim());
        if (usuario is null)
            return MensajeConfirmacion;

        var ahora = DateTime.UtcNow;
        var token = CrearTokenSeguro();
        var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
        var vigenciaMinutos = _configuracion.ExpiresInMinutes > 0
            ? _configuracion.ExpiresInMinutes
            : 30;

        await _recuperacionRepositorio.CrearAsync(
            new RecuperacionContrasena
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuario.Id,
                TokenHash = tokenHash,
                CreadoEn = ahora,
                ExpiraEn = ahora.AddMinutes(vigenciaMinutos)
            },
            ahora,
            cancellationToken);

        var enlace = CrearEnlaceRestablecimiento(token);
        await _emailSender.EnviarEnlaceRestablecimientoAsync(
            usuario.Email,
            usuario.Nombre,
            enlace,
            vigenciaMinutos,
            cancellationToken);

        return MensajeConfirmacion;
    }

    private string CrearEnlaceRestablecimiento(string token)
    {
        var urlConfigurada = _configuracion.ResetPageUrl;
        if (!Uri.TryCreate(urlConfigurada, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttps &&
             !(uri.Scheme == Uri.UriSchemeHttp && uri.IsLoopback)))
        {
            throw new EmailDeliveryException("La URL para restablecer la contraseña no está configurada correctamente.");
        }

        var builder = new UriBuilder(uri);
        var queryActual = builder.Query.TrimStart('?');
        var parametroToken = $"token={Uri.EscapeDataString(token)}";
        builder.Query = string.IsNullOrEmpty(queryActual)
            ? parametroToken
            : $"{queryActual}&{parametroToken}";

        return builder.Uri.ToString();
    }

    private static string CrearTokenSeguro()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}
