// ADMIN Marcos: HU-02-03 la cuenta está suspendida; el login responde 403 con el motivo registrado por el Administrador.
namespace Usuarios.Application.Exceptions;

public class CuentaSuspendidaException(string? motivo) : Exception("Tu cuenta está suspendida.")
{
    public string? Motivo { get; } = string.IsNullOrWhiteSpace(motivo) ? null : motivo.Trim();
}
