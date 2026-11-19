using FluentValidation;

namespace Usuarios.Application.Commands.GuardarPerfilEstudiante;

public sealed class GuardarPerfilEstudianteCommandValidator : AbstractValidator<GuardarPerfilEstudianteCommand>
{
    private static readonly string[] NivelesValidos = ["Basico", "Intermedio", "Avanzado"];

    public GuardarPerfilEstudianteCommandValidator()
    {
        RuleFor(x => x.Carrera)
            .NotEmpty().WithMessage("La carrera es obligatoria.")
            .Must(c => !string.IsNullOrWhiteSpace(c) && c.Trim().Length <= 150)
            .WithMessage("La carrera no puede exceder los 150 caracteres.");

        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage("La descripción es obligatoria.")
            .Must(d => !string.IsNullOrWhiteSpace(d) && d.Trim().Length <= 2000)
            .WithMessage("La descripción no puede exceder los 2000 caracteres.");

        RuleFor(x => x)
            .Must(x =>
                (x.SkillsCatalogo is { Count: > 0 }) ||
                (x.Habilidades is { Count: > 0 } && x.Habilidades.Any(h => !string.IsNullOrWhiteSpace(h))))
            .WithMessage("Debe especificar al menos una habilidad del catálogo o adicional.");

        RuleForEach(x => x.SkillsCatalogo).ChildRules(skill =>
        {
            skill.RuleFor(s => s.SkillId)
                .NotEmpty().WithMessage("El identificador de la habilidad del catálogo es obligatorio.");

            skill.RuleFor(s => s.NivelDominio)
                .Must(n => NivelesValidos.Contains(n))
                .WithMessage("El nivel de dominio debe ser Basico, Intermedio o Avanzado.");
        });
    }
}
