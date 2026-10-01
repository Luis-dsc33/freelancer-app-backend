using FluentValidation;
using Usuarios.Application.Interfaces;

namespace Usuarios.Application.Commands.RegistrarUsuario;

public class RegistrarUsuarioCommandValidator : AbstractValidator<RegistrarUsuarioCommand>
{
    private static readonly string[] RolesPermitidosEnRegistro = { "Estudiante", "Cliente" };
    private readonly IPasswordStrengthChecker _passwordStrengthChecker;

    public RegistrarUsuarioCommandValidator(IPasswordStrengthChecker passwordStrengthChecker)
    {
        _passwordStrengthChecker = passwordStrengthChecker;

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(150);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo es obligatorio.")
            .EmailAddress().WithMessage("El correo no tiene un formato válido.");

        // ERJ 30/09/2026 - SE AGREGARON VALIDACIONES EXTRAS PARA LAS CONTRASEÑAS
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .MaximumLength(128).WithMessage("La contraseña no puede exceder 128 caracteres.")
            .Matches("[A-Z]").WithMessage("Debe incluir al menos una letra mayúscula.")
            .Matches("[a-z]").WithMessage("Debe incluir al menos una letra minúscula.")
            .Matches("[0-9]").WithMessage("Debe incluir al menos un número.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Debe incluir al menos un carácter especial.")
            .CustomAsync(async (password, context, ct) =>
            {
                var (esDebil, razon) = await _passwordStrengthChecker.EsDebilAsync(password, ct);
                if (esDebil)
                {
                    context.AddFailure("Password", razon);
                }
            });

        RuleFor(x => x.Rol)
            .NotEmpty().WithMessage("El rol es obligatorio.")
            .Must(rol => RolesPermitidosEnRegistro.Contains(rol))
            .WithMessage("El rol debe ser 'Estudiante' o 'Cliente'.");
    }
}