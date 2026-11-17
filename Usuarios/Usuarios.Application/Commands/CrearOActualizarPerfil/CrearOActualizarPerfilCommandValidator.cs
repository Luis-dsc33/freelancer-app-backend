using FluentValidation;

namespace Usuarios.Application.Commands.CrearOActualizarPerfil;

public class CrearOActualizarPerfilCommandValidator : AbstractValidator<CrearOActualizarPerfilCommand>
{
    public CrearOActualizarPerfilCommandValidator()
    {
        RuleFor(x => x.Carrera)
            .NotEmpty().WithMessage("La carrera es obligatoria.");

        RuleFor(x => x.Habilidades)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Debe especificar al menos una habilidad.")
            .Must(h => h.Any(s => !string.IsNullOrWhiteSpace(s)))
            .WithMessage("Debe especificar al menos una habilidad.");


        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage("La descripcion es obligatoria.");
    }
}
