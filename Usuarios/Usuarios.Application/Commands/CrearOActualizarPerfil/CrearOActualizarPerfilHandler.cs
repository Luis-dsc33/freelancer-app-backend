using MediatR;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Application.Commands.CrearOActualizarPerfil;

public class CrearOActualizarPerfilHandler : IRequestHandler<CrearOActualizarPerfilCommand, Guid>
{
    private readonly IPerfilEstudianteRepository _repository;
    private readonly ICurrentUserService _currentUser;

    public CrearOActualizarPerfilHandler(
        IPerfilEstudianteRepository repository,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CrearOActualizarPerfilCommand request, CancellationToken cancellationToken)
    {
        var usuarioId = _currentUser.UsuarioId;
        var carrera = request.Carrera.Trim();
        var descripcion = request.Descripcion.Trim();
        var habilidades = request.Habilidades
            .Where(h => !string.IsNullOrWhiteSpace(h))
            .Select(h => h.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var perfilExistente = await _repository.ObtenerPorUsuarioIdAsync(usuarioId);

        if (perfilExistente != null)
        {
            perfilExistente.Carrera = carrera;
            perfilExistente.Habilidades = habilidades;
            perfilExistente.Descripcion = descripcion;

            await _repository.ActualizarAsync(perfilExistente);
            return perfilExistente.Id;
        }

        var nuevoPerfil = new PerfilEstudiante
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuarioId,
            Carrera = carrera,
            Habilidades = habilidades,
            Descripcion = descripcion
        };

        await _repository.AgregarAsync(nuevoPerfil);
        return nuevoPerfil.Id;
    }
}
