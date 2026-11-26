using MediatR;
using Resenas.Application.Dtos;

namespace Resenas.Application.Commands.CalificarEstudiante;

public sealed record CalificarEstudianteCommand(
    Guid ContratoId,
    int Puntuacion,
    string Comentario) : IRequest<ResenaDto>;
