using FluentValidation;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Commands.ResolverPostulacion;

public sealed class ResolverPostulacionCommandValidator : AbstractValidator<ResolverPostulacionCommand>
{
    public ResolverPostulacionCommandValidator()
    {
        RuleFor(x => x.PostulacionId).NotEmpty();
        RuleFor(x => x.Estado).Must(x => x is Postulacion.EstadoAceptada or Postulacion.EstadoRechazada);
    }
}
