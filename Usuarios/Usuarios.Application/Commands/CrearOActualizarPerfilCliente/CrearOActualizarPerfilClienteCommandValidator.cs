using FluentValidation;

namespace Usuarios.Application.Commands.CrearOActualizarPerfilCliente;

public sealed class CrearOActualizarPerfilClienteCommandValidator
    : AbstractValidator<CrearOActualizarPerfilClienteCommand>
{
    public CrearOActualizarPerfilClienteCommandValidator()
    {
        RuleFor(x => x.NombreEmpresa).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Telefono).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Ubicacion).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.DatosContacto).NotEmpty().MaximumLength(1000);
    }
}
