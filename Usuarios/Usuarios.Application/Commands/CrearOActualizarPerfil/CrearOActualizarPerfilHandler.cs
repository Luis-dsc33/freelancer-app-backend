using MediatR;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Application.Commands.CrearOActualizarPerfil;

public class CrearOActualizarPerfilHandler : IRequestHandler<CrearOActualizarPerfilCommand, Guid>
{
    private readonly IPerfilEstudianteRepository _repository;

    public CrearOActualizarPerfilHandler(IPerfilEstudianteRepository repository) => _repository = repository;

    public async Task<Guid> Handle(CrearOActualizarPerfilCommand request, CancellationToken cancellationToken)
    {
        var perfilExistente = await _repository.ObtenerPorUsuarioIdAsync(request.UsuarioId);

        if (perfilExistente != null)
        {
            perfilExistente.Carrera = request.Carrera;
            perfilExistente.Habilidades = request.Habilidades;
            perfilExistente.Descripcion = request.Descripcion;
            await _repository.ActualizarAsync(perfilExistente);
            return perfilExistente.Id;
        }

        var nuevoPerfil = new PerfilEstudiante
        {
            Id = Guid.NewGuid(),
            UsuarioId = request.UsuarioId,
            Carrera = request.Carrera,
            Habilidades = request.Habilidades,
            Descripcion = request.Descripcion
        };

        await _repository.AgregarAsync(nuevoPerfil);
        return nuevoPerfil.Id;
    }
}
