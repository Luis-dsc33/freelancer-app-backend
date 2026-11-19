using MediatR;

namespace Usuarios.Application.Commands.CrearOActualizarPerfilCliente;

public sealed record CrearOActualizarPerfilClienteCommand(
    string NombreEmpresa,
    string Telefono,
    string Ubicacion,
    string Descripcion,
    string DatosContacto) : IRequest<Guid>;
