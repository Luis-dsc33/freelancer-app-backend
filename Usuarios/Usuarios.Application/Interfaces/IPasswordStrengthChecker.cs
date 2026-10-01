namespace Usuarios.Application.Interfaces;

public interface IPasswordStrengthChecker
{
    /// <summary>
    /// Evalúa si la contraseña ha sido filtrada o es débil.
    /// </summary>
    Task<(bool EsDebil, string Razon)> EsDebilAsync(string password, CancellationToken ct = default);
}