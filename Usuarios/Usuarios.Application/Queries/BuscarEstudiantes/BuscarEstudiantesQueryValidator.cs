using FluentValidation;

namespace Usuarios.Application.Queries.BuscarEstudiantes;

public sealed class BuscarEstudiantesQueryValidator : AbstractValidator<BuscarEstudiantesQuery>
{
    public BuscarEstudiantesQueryValidator()
    {
        RuleFor(x => x.Pagina).GreaterThanOrEqualTo(1);
        RuleFor(x => x.TamanoPagina).InclusiveBetween(1, 50);
        RuleFor(x => x.Habilidad).MaximumLength(100);
        RuleFor(x => x.Carrera).MaximumLength(150);
    }
}
