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
        var carrera = request.Carrera.Trim();
        var descripcion = request.Descripcion.Trim();
        var habilidades = request.Habilidades
            .Where(h => !string.IsNullOrWhiteSpace(h))
            .Select(h => h.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var perfilExistente = await _repository.ObtenerPorUsuarioIdAsync(request.UsuarioId);

        if (perfilExistente != null)
        {
<<<<<<< HEAD
            perfilExistente.Carrera = request.Carrera;
            perfilExistente.Habilidades = request.Habilidades;
            perfilExistente.Descripcion = request.Descripcion;

            perfilExistente.Carrera = carrera;
            perfilExistente.Habilidades = habilidades;
            perfilExistente.Descripcion = descripcion;

            await _repository.ActualizarAsync(perfilExistente);
            return perfilExistente.Id;
        }

        var nuevoPerfil = new PerfilEstudiante
        {
            Id = Guid.NewGuid(),
            UsuarioId = request.UsuarioId,
<<<<<<< HEAD
            Carrera = request.Carrera,
            Habilidades = request.Habilidades,
            Descripcion = request.Descripcion

            Carrera = carrera,
            Habilidades = habilidades,
            Descripcion = descripcion

        };

        await _repository.AgregarAsync(nuevoPerfil);
        return nuevoPerfil.Id;
    }

}

}

