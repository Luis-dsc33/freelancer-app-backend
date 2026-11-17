// ADMIN Marcos: HU-21 Ocultar oferta / HU-26 Restaurar oferta.
// El motivo y quién lo hizo se registran en la bitácora del servicio Admin.
using Marketplace.Application.Queries.ListarOfertasAdmin;
using MediatR;

namespace Marketplace.Application.Commands.CambiarVisibilidadOferta;

public record CambiarVisibilidadOfertaCommand(Guid OfertaId, bool Ocultar) : IRequest<OfertaAdminDto>;
