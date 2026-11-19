using Chat.Application.Dtos;
using MediatR;

namespace Chat.Application.Commands.AbrirConversacion;

public sealed record AbrirConversacionCommand(
    Guid ProyectoId,
    Guid? PostulacionId = null,
    Guid? ContratoId = null) : IRequest<AbrirConversacionResult>;
