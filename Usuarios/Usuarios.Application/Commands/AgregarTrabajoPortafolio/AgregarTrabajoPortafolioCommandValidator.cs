using FluentValidation;

namespace Usuarios.Application.Commands.AgregarTrabajoPortafolio;

public class AgregarTrabajoPortafolioCommandValidator : AbstractValidator<AgregarTrabajoPortafolioCommand>
{
    public AgregarTrabajoPortafolioCommandValidator()
    {
        RuleFor(c => c.UsuarioId).NotEmpty();
        RuleFor(c => c.Titulo).NotEmpty().WithMessage("El título es obligatorio.")
            .MaximumLength(150).WithMessage("El título admite hasta 150 caracteres.");
        RuleFor(c => c.Descripcion).NotEmpty().WithMessage("La descripción es obligatoria.")
            .MaximumLength(2000).WithMessage("La descripción admite hasta 2000 caracteres.");
        RuleFor(c => c.Enlace).MaximumLength(2048)
            .Must(enlace => string.IsNullOrWhiteSpace(enlace) ||
                (Uri.TryCreate(enlace.Trim(), UriKind.Absolute, out var uri) &&
                 (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) &&
                 !string.IsNullOrWhiteSpace(uri.Host)))
            .WithMessage("El enlace debe ser una URL válida con http:// o https://.");
    }
}
