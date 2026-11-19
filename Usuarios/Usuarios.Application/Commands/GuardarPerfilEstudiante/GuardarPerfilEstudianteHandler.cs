using MediatR;
using Usuarios.Application.Interfaces;
using Usuarios.Application.Queries.ObtenerPerfil;
using Usuarios.Domain.Entities;

namespace Usuarios.Application.Commands.GuardarPerfilEstudiante;

public sealed class GuardarPerfilEstudianteHandler(
    IPerfilEstudianteRepository repository,
    IUsuarioRepository usuarios,
    ICatalogoSkillsMarketplaceClient catalogoClient,
    ICurrentUserService currentUser)
    : IRequestHandler<GuardarPerfilEstudianteCommand, PerfilEstudianteDto>
{
    public async Task<PerfilEstudianteDto> Handle(
        GuardarPerfilEstudianteCommand request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

        if (!currentUser.EsEstudiante && !string.IsNullOrWhiteSpace(currentUser.Role))
            throw new UnauthorizedAccessException("Solo las cuentas de estudiante pueden editar este perfil.");

        var usuarioId = currentUser.UserId;
        var usuario = await usuarios.ObtenerPorIdAsync(usuarioId, cancellationToken);
        var nombre = usuario?.Nombre ?? currentUser.Nombre;

        var carrera = request.Carrera.Trim();
        var descripcion = request.Descripcion.Trim();

        var skillsInput = (request.SkillsCatalogo ?? [])
            .GroupBy(s => s.SkillId)
            .Select(g => g.Last())
            .ToList();

        IReadOnlyDictionary<Guid, SkillCatalogoValidadaDto> catalogoValidado =
            new Dictionary<Guid, SkillCatalogoValidadaDto>();

        if (skillsInput.Count > 0)
        {
            catalogoValidado = await catalogoClient.ValidarYObtenerSkillsAsync(
                skillsInput.Select(s => s.SkillId).ToList(),
                cancellationToken);
        }

        var habilidadesLibres = (request.Habilidades ?? [])
            .Where(h => !string.IsNullOrWhiteSpace(h))
            .Select(h => h.Trim());

        var nombresCatalogo = catalogoValidado.Values
            .Select(s => s.NombreSkill.Trim());

        var habilidadesCombinadas = nombresCatalogo
            .Concat(habilidadesLibres)
            .Where(h => !string.IsNullOrWhiteSpace(h))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var perfil = await repository.ObtenerPorUsuarioIdAsync(usuarioId);
        var esNuevo = perfil is null;

        if (perfil is null)
        {
            perfil = new PerfilEstudiante
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuarioId,
                Carrera = carrera,
                Descripcion = descripcion,
                Habilidades = habilidadesCombinadas
            };
        }
        else
        {
            perfil.Carrera = carrera;
            perfil.Descripcion = descripcion;
            perfil.Habilidades = habilidadesCombinadas;
        }

        var nuevasSkills = skillsInput.Select(s =>
        {
            var oficial = catalogoValidado[s.SkillId];
            return new EstudianteSkill(
                perfil.Id,
                s.SkillId,
                oficial.NombreSkill,
                oficial.Categoria,
                s.NivelDominio);
        }).ToList();

        perfil.ActualizarSkillsCatalogo(nuevasSkills);

        if (esNuevo)
        {
            await repository.AgregarAsync(perfil);
        }
        else
        {
            await repository.ActualizarAsync(perfil);
        }

        var obligatorios = PerfilEstudiante.CamposObligatorios.ToList();
        var faltantes = perfil.ObtenerCamposFaltantes();

        return new PerfilEstudianteDto
        {
            Id = perfil.Id,
            UsuarioId = perfil.UsuarioId,
            Nombre = nombre,
            Carrera = perfil.Carrera,
            Habilidades = perfil.Habilidades,
            SkillsCatalogo = perfil.SkillsCatalogo
                .Select(s => new EstudianteSkillDto(s.SkillId, s.NombreSkill, s.Categoria, s.NivelDominio))
                .ToList(),
            Descripcion = perfil.Descripcion,
            EsPerfilCompleto = faltantes.Count == 0,
            CamposObligatorios = obligatorios,
            CamposFaltantes = faltantes
        };
    }
}
