using FluentValidation;

namespace Marketplace.Application.Commands.PostularAOferta;

public sealed class PostularAOfertaCommandValidator : AbstractValidator<PostularAOfertaCommand>
{
    public PostularAOfertaCommandValidator()
    {
        RuleFor(x => x.OfertaId)
            .NotEmpty().WithMessage("La oferta es obligatoria.");

        RuleFor(x => x.Mensaje)
            .NotEmpty().WithMessage("El mensaje de postulación es obligatorio.")
            .Must(m => !string.IsNullOrWhiteSpace(m) && m.Trim().Length >= 10)
            .WithMessage("Describe tu propuesta en al menos 10 caracteres.")
            .Must(m => string.IsNullOrWhiteSpace(m) || m.Trim().Length <= 2000)
            .WithMessage("La propuesta no puede exceder los 2000 caracteres.");
    }
}
