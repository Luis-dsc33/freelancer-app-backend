using MediatR;
using Resenas.Application.Dtos;

namespace Resenas.Application.Commands.CalificarCliente;

public sealed record CalificarClienteCommand(
    Guid ContratoId,
    int Puntuacion,
    string Comentario) : IRequest<ResenaDto>;
