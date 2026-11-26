using FluentValidation;

namespace Resenas.Application.Commands.CalificarEstudiante;

public sealed class CalificarEstudianteCommandValidator : AbstractValidator<CalificarEstudianteCommand>
{
    public CalificarEstudianteCommandValidator()
    {
        RuleFor(x => x.ContratoId)
            .NotEmpty().WithMessage("El identificador del contrato es obligatorio.");

        RuleFor(x => x.Puntuacion)
            .InclusiveBetween(1, 5).WithMessage("La puntuación debe estar entre 1 y 5 estrellas.");

        RuleFor(x => x.Comentario)
            .NotEmpty().WithMessage("El comentario es obligatorio.")
            .Must(c => !string.IsNullOrWhiteSpace(c) && c.Trim().Length <= 2000)
            .WithMessage("El comentario no puede exceder los 2000 caracteres.");
    }
}
