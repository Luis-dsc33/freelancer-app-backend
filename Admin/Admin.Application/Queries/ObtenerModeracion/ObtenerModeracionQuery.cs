// ADMIN Marcos: HU-21/HU-26 datos de la sección Moderación: cuentas suspendidas y ofertas, cada una con
// el motivo de su medida vigente. Si un servicio no responde, solo su lista queda en null.
using Admin.Application.Commands.AplicarMedidaModeracion;
using Admin.Application.Dtos;
using Admin.Application.Interfaces;
using Admin.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Admin.Application.Queries.ObtenerModeracion;

public record ObtenerModeracionQuery : IRequest<ModeracionDto>;

public class ObtenerModeracionHandler(
    IUsuariosAdminClient usuarios,
    IMarketplaceAdminClient marketplace,
    IAccionAdministrativaRepository bitacora,
    ILogger<ObtenerModeracionHandler> logger)
    : IRequestHandler<ObtenerModeracionQuery, ModeracionDto>
{
    public async Task<ModeracionDto> Handle(ObtenerModeracionQuery request, CancellationToken ct)
    {
        IReadOnlyList<CuentaSuspendidaDto>? cuentas = null;
        IReadOnlyList<OfertaModeracionDto>? ofertas = null;

        try
        {
            var suspendidas = (await usuarios.BuscarAsync(null, null, "Suspendido", 1, 50, ct)).Items;
            var medidas = await bitacora.ObtenerUltimasAsync(AccionAdministrativa.Suspender, suspendidas.Select(u => u.Id).ToList(), ct);
            cuentas = suspendidas
                .Select(u => new CuentaSuspendidaDto(u, medidas.TryGetValue(u.Id, out var m) ? AplicarMedidaModeracionHandler.ADto(m) : null))
                .ToList();
        }
        catch (Exception ex) when (!ct.IsCancellationRequested)
        {
            logger.LogWarning(ex, "Usuarios no disponible para Moderación");
        }

        try
        {
            var lista = await marketplace.ListarOfertasAsync(null, ct);
            var medidas = await bitacora.ObtenerUltimasAsync(AccionAdministrativa.Ocultar, lista.Select(o => o.Id).ToList(), ct);
            ofertas = lista
                .Select(o => new OfertaModeracionDto(o,
                    o.Estado == "Oculta" && medidas.TryGetValue(o.Id, out var m) ? AplicarMedidaModeracionHandler.ADto(m) : null))
                .ToList();
        }
        catch (Exception ex) when (!ct.IsCancellationRequested)
        {
            logger.LogWarning(ex, "Marketplace no disponible para Moderación");
        }

        return new ModeracionDto(cuentas, ofertas);
    }
}
