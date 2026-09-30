using FluentValidation;

namespace Usuarios.Application.Commands.CrearOActualizarPerfil;

public class CrearOActualizarPerfilCommandValidator : AbstractValidator<CrearOActualizarPerfilCommand>
{
    public CrearOActualizarPerfilCommandValidator()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty().WithMessage("El usuario es requerido.");

        RuleFor(x => x.Carrera)
            .NotEmpty().WithMessage("La carrera es obligatoria.");

        RuleFor(x => x.Habilidades)
            .NotEmpty().WithMessage("Debe especificar al menos una habilidad.");

        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage("La descripcion es obligatoria.");
    }
}
