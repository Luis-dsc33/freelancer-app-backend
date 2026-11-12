using MediatR;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Application.Queries.ObtenerPerfil;

public class ObtenerPerfilHandler : IRequestHandler<ObtenerPerfilQuery, PerfilEstudianteDto>
{
    private readonly IPerfilEstudianteRepository _repository;
    private readonly ICurrentUserService _currentUser;

    public ObtenerPerfilHandler(
        IPerfilEstudianteRepository repository,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<PerfilEstudianteDto> Handle(ObtenerPerfilQuery request, CancellationToken cancellationToken)
    {
        var usuarioId = _currentUser.UsuarioId;
        var perfil = await _repository.ObtenerPorUsuarioIdAsync(usuarioId);
        var obligatorios = PerfilEstudiante.CamposObligatorios.ToList();

        // Primer ingreso: todavía no existe perfil, todos los campos están pendientes
        if (perfil is null)
        {
            return new PerfilEstudianteDto
            {
                Id = null,
                UsuarioId = usuarioId,
                EsPerfilCompleto = false,
                CamposObligatorios = obligatorios,
                CamposFaltantes = obligatorios.ToList()
            };
        }

        var faltantes = perfil.ObtenerCamposFaltantes();

        return new PerfilEstudianteDto
        {
            Id = perfil.Id,
            UsuarioId = perfil.UsuarioId,
            Carrera = perfil.Carrera ?? string.Empty,
            Habilidades = perfil.Habilidades ?? new List<string>(),
            Descripcion = perfil.Descripcion ?? string.Empty,
            EsPerfilCompleto = faltantes.Count == 0,
            CamposObligatorios = obligatorios,
            CamposFaltantes = faltantes
        };
    }
}