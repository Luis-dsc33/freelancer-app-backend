using FluentValidation;

namespace Marketplace.Application.Commands.PublicarOferta;

public sealed class PublicarOfertaCommandValidator : AbstractValidator<PublicarOfertaCommand>
{
    public PublicarOfertaCommandValidator()
    {
        RuleFor(command => command.Titulo)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(command => command.Descripcion)
            .NotEmpty()
            .MaximumLength(4000);

        RuleFor(command => command.Disciplina)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.Presupuesto)
            .GreaterThanOrEqualTo(0.01m)
            .WithMessage("El presupuesto debe ser mayor que cero.");

        RuleFor(command => command.FechaLimite)
            .Must(fechaLimite => fechaLimite >= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("La fecha límite no puede ser anterior a la fecha de publicación.");

        RuleFor(command => command.ClienteId)
            .NotEqual(Guid.Empty);

        RuleFor(command => command.NombreCliente)
            .NotEmpty()
            .MaximumLength(150);
    }
}
