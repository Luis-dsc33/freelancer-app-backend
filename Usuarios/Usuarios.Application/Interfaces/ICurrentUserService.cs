namespace Usuarios.Application.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
    bool IsAuthenticated { get; }
    string? Role => null;
    string? Nombre => null;
    bool EsEstudiante => string.Equals(Role, "Estudiante", StringComparison.OrdinalIgnoreCase);
    bool EsCliente => string.Equals(Role, "Cliente", StringComparison.OrdinalIgnoreCase);
}
