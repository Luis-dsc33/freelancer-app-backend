using Microsoft.Extensions.Configuration;
using Usuarios.Application.Interfaces;

namespace Usuarios.Infrastructure.Security;

public class PasswordResetSettings : IPasswordResetSettings
{
    public PasswordResetSettings(IConfiguration configuration)
    {
        ExpiresInMinutes = int.TryParse(configuration["PasswordReset:ExpiresInMinutes"], out var minutos) && minutos > 0
            ? minutos
            : 30;
        ResetPageUrl = configuration["PasswordReset:ResetPageUrl"] ?? string.Empty;
    }

    public int ExpiresInMinutes { get; }
    public string ResetPageUrl { get; }
}
