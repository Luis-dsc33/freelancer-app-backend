using Usuarios.Application.Interfaces;
using Zxcvbn;

namespace Usuarios.Infrastructure.Security;

public class ZxcvbnPasswordStrengthChecker : IPasswordStrengthChecker
{
    private const int PuntajeMinimo = 2;

    public Task<(bool EsDebil, string Razon)> EsDebilAsync(string password, CancellationToken ct = default)
    {
        var resultado = Core.EvaluatePassword(password);

        if (resultado.Score < PuntajeMinimo)
        {
            var advertencia = resultado.Feedback?.Warning is { Length: > 0 } adv
                ? adv
                : "La contraseña es demasiado predecible.";

            return Task.FromResult((true, advertencia));
        }

        return Task.FromResult((false, string.Empty));
    }
}