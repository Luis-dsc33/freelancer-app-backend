using System.Security.Cryptography;
using System.Text;
using MediatR;
using Usuarios.Application.Interfaces;

namespace Usuarios.Application.Commands.RestablecerContrasena;

public class RestablecerContrasenaHandler : IRequestHandler<RestablecerContrasenaCommand, Unit>
{
    private const string MensajeTokenInvalido =
        "El enlace no es válido, venció o ya fue utilizado. Solicita uno nuevo.";

    private readonly IRecuperacionContrasenaRepository _recuperacionRepositorio;
    private readonly IPasswordHasher _passwordHasher;

    public RestablecerContrasenaHandler(
        IRecuperacionContrasenaRepository recuperacionRepositorio,
        IPasswordHasher passwordHasher)
    {
        _recuperacionRepositorio = recuperacionRepositorio;
        _passwordHasher = passwordHasher;
    }

    public async Task<Unit> Handle(RestablecerContrasenaCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(request.Token)));
        var ahora = DateTime.UtcNow;
        if (!await _recuperacionRepositorio.EsTokenValidoAsync(tokenHash, ahora, cancellationToken))
            throw new InvalidOperationException(MensajeTokenInvalido);

        var passwordHash = _passwordHasher.Hash(request.Password);

        var restablecida = await _recuperacionRepositorio.RestablecerContrasenaAsync(
            tokenHash,
            passwordHash,
            ahora,
            cancellationToken);

        if (!restablecida)
            throw new InvalidOperationException(MensajeTokenInvalido);

        return Unit.Value;
    }
}
