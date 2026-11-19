using FluentValidation;

namespace Usuarios.Application.Commands.SolicitarRecuperacion;

public class SolicitarRecuperacionCommandValidator : AbstractValidator<SolicitarRecuperacionCommand>
{
    public SolicitarRecuperacionCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo es obligatorio.")
            .EmailAddress().WithMessage("El correo no tiene un formato válido.");
    }
}
