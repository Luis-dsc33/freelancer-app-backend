using FluentValidation;

namespace Usuarios.Application.Commands.IniciarSesion;

public class IniciarSesionCommandValidator : AbstractValidator<IniciarSesionCommand>
{
    public IniciarSesionCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
        // Aquí solo formato — nunca "no cumple la política de X caracteres",
        // porque eso filtraría información útil para un atacante probando contraseñas.
    }
}