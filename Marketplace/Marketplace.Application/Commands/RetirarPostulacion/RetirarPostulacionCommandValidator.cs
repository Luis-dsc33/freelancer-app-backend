using FluentValidation;

namespace Marketplace.Application.Commands.RetirarPostulacion;

public sealed class RetirarPostulacionCommandValidator : AbstractValidator<RetirarPostulacionCommand>
{
    public RetirarPostulacionCommandValidator()
    {
        RuleFor(x => x.PostulacionId)
            .NotEmpty().WithMessage("El identificador de la postulación es obligatorio.");
    }
}
