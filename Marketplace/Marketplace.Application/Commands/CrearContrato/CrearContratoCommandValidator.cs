using FluentValidation;

namespace Marketplace.Application.Commands.CrearContrato;

public sealed class CrearContratoCommandValidator : AbstractValidator<CrearContratoCommand>
{
    public CrearContratoCommandValidator()
    {
        RuleFor(x => x.PostulacionId).NotEmpty();
        RuleFor(x => x.ImporteAcordado).GreaterThan(0);
        RuleFor(x => x.FechaCompromiso).GreaterThanOrEqualTo(x => x.FechaInicio);
    }
}
