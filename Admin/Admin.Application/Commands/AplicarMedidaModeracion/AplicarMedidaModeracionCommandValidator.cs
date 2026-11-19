// ADMIN Marcos: HU-21-03 / HU-26-03 sin motivo no se aplica la medida.
using Admin.Domain.Entities;
using FluentValidation;

namespace Admin.Application.Commands.AplicarMedidaModeracion;

public class AplicarMedidaModeracionCommandValidator : AbstractValidator<AplicarMedidaModeracionCommand>
{
    private static readonly string[] Tipos =
        [AccionAdministrativa.Suspender, AccionAdministrativa.Reactivar, AccionAdministrativa.Ocultar, AccionAdministrativa.Restaurar];

    public AplicarMedidaModeracionCommandValidator()
    {
        RuleFor(x => x.Motivo)
            .Must(m => !string.IsNullOrWhiteSpace(m)).WithMessage("El motivo es obligatorio.")
            .MaximumLength(500).WithMessage("El motivo no puede superar 500 caracteres.");
        RuleFor(x => x.TipoAccion).Must(t => Tipos.Contains(t)).WithMessage("Tipo de medida no válido.");
        RuleFor(x => x.RecursoId).NotEmpty();
    }
}
