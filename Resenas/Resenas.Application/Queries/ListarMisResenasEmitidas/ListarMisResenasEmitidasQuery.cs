using MediatR;
using Resenas.Application.Dtos;

namespace Resenas.Application.Queries.ListarMisResenasEmitidas;

public sealed record ListarMisResenasEmitidasQuery : IRequest<IReadOnlyList<ResenaDto>>;
