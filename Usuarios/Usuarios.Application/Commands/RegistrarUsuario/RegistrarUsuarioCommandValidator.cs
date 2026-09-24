using FluentValidation;

namespace Usuarios.Application.Commands.RegistrarUsuario;

public class RegistrarUsuarioCommandValidator : AbstractValidator<RegistrarUsuarioCommand>
{
    private static readonly string[] RolesPermitidosEnRegistro = { "Estudiante", "Cliente" };

    public RegistrarUsuarioCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(150);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo es obligatorio.")
            .EmailAddress().WithMessage("El correo no tiene un formato válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.");
        // TODO: ajustar longitud/reglas cuando el equipo acuerde la política de contraseña (P-02)

        RuleFor(x => x.Rol)
            .NotEmpty().WithMessage("El rol es obligatorio.")
            .Must(rol => RolesPermitidosEnRegistro.Contains(rol))
            .WithMessage("El rol debe ser 'Estudiante' o 'Cliente'.");
        // "Administrador" nunca se autoasigna desde el registro público — HU-01 + HU-22
    }
}