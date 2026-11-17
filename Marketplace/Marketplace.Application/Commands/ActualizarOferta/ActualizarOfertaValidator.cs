using FluentValidation;
using Marketplace.Application.Commands.CrearOferta;
using Marketplace.Domain.Enums;

namespace Marketplace.Application.Commands.ActualizarOferta;

public sealed class ActualizarOfertaCommandValidator : AbstractValidator<ActualizarOfertaCommand>
{
    public ActualizarOfertaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.Disciplina).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Moneda).NotEmpty().Length(3);
        RuleFor(x => x.FechaLimite).Must(x => x >= DateOnly.FromDateTime(DateTime.UtcNow));
        RuleFor(x => x.FechaEntregaEstimada).GreaterThanOrEqualTo(x => x.FechaLimite).When(x => x.FechaEntregaEstimada.HasValue);
        RuleFor(x => x.HabilidadesRequeridas).NotEmpty();
        RuleFor(x => x.CantidadVacantes).GreaterThan(0);
        RuleFor(x => x.Ubicacion).NotEmpty().When(x => x.Modalidad is ModalidadProyecto.Presencial or ModalidadProyecto.Hibrido);
        RuleFor(x => x).Must(PresupuestoValido).WithMessage("El presupuesto no coincide con la modalidad seleccionada.");
        RuleFor(x => x.HabilidadesDeseables).Must((command, skills) => !skills.Intersect(command.HabilidadesRequeridas).Any());
        RuleForEach(x => x.Entregables).NotEmpty().MaximumLength(1000);
        RuleForEach(x => x.HitosPago).SetValidator(new HitoPagoInputValidator());
    }

    private static bool PresupuestoValido(ActualizarOfertaCommand command) => command.TipoPresupuesto switch
    {
        TipoPresupuesto.Fijo => command.Presupuesto is > 0 && command.PresupuestoMinimo is null && command.PresupuestoMaximo is null,
        TipoPresupuesto.Rango => command.Presupuesto is null && command.PresupuestoMinimo is > 0 && command.PresupuestoMaximo >= command.PresupuestoMinimo,
        _ => false
    };
}
