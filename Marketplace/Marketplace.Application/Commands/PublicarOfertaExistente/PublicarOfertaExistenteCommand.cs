using Marketplace.Application.Queries.ListarOportunidades;
using MediatR;

namespace Marketplace.Application.Commands.PublicarOfertaExistente;

public sealed record PublicarOfertaExistenteCommand(Guid Id) : IRequest<OportunidadProyectoDto>;
