using MediatR;
using Usuarios.Application.Interfaces;
using Usuarios.Domain.Entities;

namespace Usuarios.Application.Queries.ObtenerPerfil;

public class ObtenerPerfilHandler : IRequestHandler<ObtenerPerfilQuery, PerfilEstudianteDto>
{
    private readonly IPerfilEstudianteRepository _repository;
    private readonly IUsuarioRepository _usuarios;
    private readonly ICurrentUserService _currentUser;

    public ObtenerPerfilHandler(
        IPerfilEstudianteRepository repository,
        IUsuarioRepository usuarios,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _usuarios = usuarios;
        _currentUser = currentUser;
    }

    public async Task<PerfilEstudianteDto> Handle(ObtenerPerfilQuery request, CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated || _currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

        var usuarioId = _currentUser.UserId;
        var usuario = await _usuarios.ObtenerPorIdAsync(usuarioId, cancellationToken);
        var nombre = usuario?.Nombre ?? _currentUser.Nombre;
        var perfil = await _repository.ObtenerPorUsuarioIdAsync(usuarioId);
        var obligatorios = PerfilEstudiante.CamposObligatorios.ToList();

        // Primer ingreso: todavía no existe perfil, todos los campos están pendientes
        if (perfil is null)
        {
            return new PerfilEstudianteDto
            {
                Id = null,
                UsuarioId = usuarioId,
                Nombre = nombre,
                EsPerfilCompleto = false,
                CamposObligatorios = obligatorios,
                CamposFaltantes = obligatorios.ToList()
            };
        }

        var faltantes = perfil.ObtenerCamposFaltantes();
        var skillsCatalogo = (perfil.SkillsCatalogo ?? [])
            .Select(s => new EstudianteSkillDto(s.SkillId, s.NombreSkill, s.Categoria, s.NivelDominio))
            .ToList();

        return new PerfilEstudianteDto
        {
            Id = perfil.Id,
            UsuarioId = perfil.UsuarioId,
            Nombre = nombre,
            Carrera = perfil.Carrera ?? string.Empty,
            Habilidades = perfil.Habilidades ?? new List<string>(),
            SkillsCatalogo = skillsCatalogo,
            Descripcion = perfil.Descripcion ?? string.Empty,
            EsPerfilCompleto = faltantes.Count == 0,
            CamposObligatorios = obligatorios,
            CamposFaltantes = faltantes
        };
    }
}