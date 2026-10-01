using FluentValidation;

namespace Usuarios.Application.Commands.EliminarTrabajoPortafolio;

public class EliminarTrabajoPortafolioCommandValidator : AbstractValidator<EliminarTrabajoPortafolioCommand>
{
    public EliminarTrabajoPortafolioCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.UsuarioId).NotEmpty();
    }
}
