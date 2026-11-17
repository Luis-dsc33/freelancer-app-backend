using FluentValidation;
using Marketplace.Domain.Enums;

namespace Marketplace.Application.Commands.CrearOferta;

public sealed class CrearOfertaCommandValidator : AbstractValidator<CrearOfertaCommand>
{
    public CrearOfertaCommandValidator()
    {
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.Disciplina).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Moneda).NotEmpty().Length(3);
        RuleFor(x => x.FechaLimite).Must(x => x >= DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("La fecha límite no puede ser anterior a hoy.");
        RuleFor(x => x.FechaEntregaEstimada).GreaterThanOrEqualTo(x => x.FechaLimite).When(x => x.FechaEntregaEstimada.HasValue);
        RuleFor(x => x.HabilidadesRequeridas).NotEmpty().WithMessage("Debe indicar al menos una habilidad requerida.");
        RuleFor(x => x.CantidadVacantes).GreaterThan(0);
        RuleFor(x => x.Ubicacion).NotEmpty().When(x => x.Modalidad is ModalidadProyecto.Presencial or ModalidadProyecto.Hibrido);
        RuleFor(x => x).Must(PresupuestoValido).WithMessage("El presupuesto no coincide con la modalidad seleccionada.");
        RuleFor(x => x.HabilidadesDeseables).Must((command, skills) => !skills.Intersect(command.HabilidadesRequeridas).Any())
            .WithMessage("Una habilidad no puede ser requerida y deseable al mismo tiempo.");
        RuleForEach(x => x.Entregables).NotEmpty().MaximumLength(1000);
        RuleForEach(x => x.HitosPago).SetValidator(new HitoPagoInputValidator());
    }

    private static bool PresupuestoValido(CrearOfertaCommand command) => command.TipoPresupuesto switch
    {
        TipoPresupuesto.Fijo => command.Presupuesto is > 0 && command.PresupuestoMinimo is null && command.PresupuestoMaximo is null,
        TipoPresupuesto.Rango => command.Presupuesto is null && command.PresupuestoMinimo is > 0 && command.PresupuestoMaximo >= command.PresupuestoMinimo,
        _ => false
    };
}

public sealed class HitoPagoInputValidator : AbstractValidator<HitoPagoInput>
{
    public HitoPagoInputValidator()
    {
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.Orden).GreaterThanOrEqualTo(0);
        RuleFor(x => x).Must(x => (x.Porcentaje.HasValue ? 1 : 0) + (x.Importe.HasValue ? 1 : 0) == 1)
            .WithMessage("Cada hito debe indicar porcentaje o importe, pero no ambos.");
        RuleFor(x => x.Porcentaje).InclusiveBetween(0.01m, 100m).When(x => x.Porcentaje.HasValue);
        RuleFor(x => x.Importe).GreaterThan(0).When(x => x.Importe.HasValue);
    }
}
