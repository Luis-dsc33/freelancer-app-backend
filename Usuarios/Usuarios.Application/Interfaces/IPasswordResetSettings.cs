namespace Usuarios.Application.Interfaces;

public interface IPasswordResetSettings
{
    int ExpiresInMinutes { get; }
    string ResetPageUrl { get; }
}
