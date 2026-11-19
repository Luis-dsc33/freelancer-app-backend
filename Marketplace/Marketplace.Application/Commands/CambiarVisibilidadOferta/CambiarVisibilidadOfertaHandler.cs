// ADMIN Marcos: ocultar usa el campo Estado que ya existe (sin migración nueva).
// Como el tablero de estudiantes solo muestra ofertas "Abierto", una oferta "Oculta" deja de ser visible.
using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.ListarOfertasAdmin;
using Marketplace.Domain.Entities;
using MediatR;

namespace Marketplace.Application.Commands.CambiarVisibilidadOferta;

public class CambiarVisibilidadOfertaHandler(IProyectoAdminRepository repositorio)
    : IRequestHandler<CambiarVisibilidadOfertaCommand, OfertaAdminDto>
{
    public const string EstadoOculta = "Oculta";

    public async Task<OfertaAdminDto> Handle(CambiarVisibilidadOfertaCommand request, CancellationToken ct)
    {
        var oferta = await repositorio.ObtenerParaActualizarAsync(request.OfertaId, ct)
                     ?? throw new InvalidOperationException("La oferta no existe."); // el middleware lo traduce a 400

        if (request.Ocultar)
        {
            if (oferta.Estado == EstadoOculta)
                throw new InvalidOperationException("La oferta ya está oculta."); // HU-21-05
            oferta.Estado = EstadoOculta;
        }
        else
        {
            if (oferta.Estado != EstadoOculta)
                throw new InvalidOperationException("La oferta no está oculta."); // HU-26-04
            oferta.Estado = Proyecto.EstadoAbierto;
        }

        await repositorio.GuardarCambiosAsync(ct);
        return ListarOfertasAdminHandler.ADto(oferta);
    }
}
