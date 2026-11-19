using FluentValidation;

namespace Chat.Application.Commands.AbrirConversacion;

public sealed class AbrirConversacionCommandValidator : AbstractValidator<AbrirConversacionCommand>
{
    public AbrirConversacionCommandValidator()
    {
        RuleFor(x => x.ProyectoId)
            .NotEmpty().WithMessage("El identificador del proyecto es obligatorio.");
    }
}
