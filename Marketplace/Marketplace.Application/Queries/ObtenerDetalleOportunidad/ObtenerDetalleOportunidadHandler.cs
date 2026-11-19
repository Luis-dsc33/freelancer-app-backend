using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.ListarOportunidades;
using Marketplace.Domain.Entities;
using MediatR;

namespace Marketplace.Application.Queries.ObtenerDetalleOportunidad;

public sealed class ObtenerDetalleOportunidadHandler(
    IProyectoRepository proyectos,
    IPostulacionRepository postulaciones,
    IContratoRepository contratos,
    ICurrentUserService currentUser)
    : IRequestHandler<ObtenerDetalleOportunidadQuery, OportunidadProyectoDto>
{
    public async Task<OportunidadProyectoDto> Handle(
        ObtenerDetalleOportunidadQuery request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

        var proyecto = await proyectos.ObtenerPorIdAsync(request.ProyectoId, cancellationToken)
            ?? throw new KeyNotFoundException("La oportunidad no existe.");

        var yaPostulado = await postulaciones.ExisteParaEstudianteAsync(
            proyecto.Id,
            currentUser.UserId,
            cancellationToken);

        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);
        var estaDisponible = (proyecto.Estado == Proyecto.EstadoAbierto || proyecto.Estado == Proyecto.EstadoPublicada)
            && proyecto.FechaLimite >= hoy;

        if (currentUser.EsEstudiante)
        {
            if (!estaDisponible && !yaPostulado)
            {
                var contrato = await contratos.ObtenerParaEstudianteEnProyectoAsync(
                    proyecto.Id,
                    currentUser.UserId,
                    cancellationToken);

                if (contrato is null)
                    throw new KeyNotFoundException("Esta oportunidad no existe o ya no está disponible para postulación.");
            }
        }
        else if (currentUser.EsCliente && proyecto.ClienteId != currentUser.UserId && !estaDisponible)
        {
            throw new KeyNotFoundException("La oportunidad no existe o no está disponible.");
        }

        return ProyectoMapper.ToDto(proyecto, yaPostulado);
    }
}
