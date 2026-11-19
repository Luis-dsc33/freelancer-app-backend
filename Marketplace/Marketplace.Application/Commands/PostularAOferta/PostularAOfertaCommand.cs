using Marketplace.Application.Queries.ListarMisPostulaciones;
using MediatR;

namespace Marketplace.Application.Commands.PostularAOferta;

public sealed record PostularAOfertaCommand(
    Guid OfertaId,
    string Mensaje) : IRequest<PostulacionEstudianteDto>;
