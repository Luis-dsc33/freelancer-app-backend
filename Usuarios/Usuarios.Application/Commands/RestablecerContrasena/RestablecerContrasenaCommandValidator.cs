using FluentValidation;

namespace Usuarios.Application.Commands.RestablecerContrasena;

public class RestablecerContrasenaCommandValidator : AbstractValidator<RestablecerContrasenaCommand>
{
    public RestablecerContrasenaCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("El enlace de recuperación no es válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.");

        RuleFor(x => x.ConfirmarPassword)
            .NotEmpty().WithMessage("La confirmación de contraseña es obligatoria.")
            .Equal(x => x.Password).WithMessage("Las contraseñas no coinciden.");
    }
}
