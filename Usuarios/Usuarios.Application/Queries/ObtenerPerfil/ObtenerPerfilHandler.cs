using MediatR;
using Usuarios.Application.Interfaces;

namespace Usuarios.Application.Queries.ObtenerPerfil;

public class ObtenerPerfilHandler : IRequestHandler<ObtenerPerfilQuery, PerfilEstudianteDto?>
{
    private readonly IPerfilEstudianteRepository _repository;

    public ObtenerPerfilHandler(IPerfilEstudianteRepository repository)
    {
        _repository = repository;
    }

    public async Task<PerfilEstudianteDto?> Handle(ObtenerPerfilQuery request, CancellationToken cancellationToken)
    {
        var perfil = await _repository.ObtenerPorUsuarioIdAsync(request.UsuarioId);

        if (perfil == null)
        {
            return new PerfilEstudianteDto
            {
                Id = null,
                UsuarioId = request.UsuarioId,
                Carrera = string.Empty,
                Habilidades = new List<string>(),
                Descripcion = string.Empty,
                EsPerfilCompleto = false,
                CamposObligatorios = new List<string> { "Carrera", "Habilidades", "Descripcion" }
            };
        }

        bool esCompleto = !string.IsNullOrWhiteSpace(perfil.Carrera) &&
                           perfil.Habilidades != null && perfil.Habilidades.Any() &&
                           !string.IsNullOrWhiteSpace(perfil.Descripcion);

        return new PerfilEstudianteDto
        {
            Id = perfil.Id,
            UsuarioId = perfil.UsuarioId,
            Carrera = perfil.Carrera ?? string.Empty,
            Habilidades = perfil.Habilidades ?? new List<string>(),
            Descripcion = perfil.Descripcion ?? string.Empty,
            EsPerfilCompleto = esCompleto,
            CamposObligatorios = new List<string> { "Carrera", "Habilidades", "Descripcion" }
        };
    }
}
