using MediatR;
using Resenas.Application.Dtos;
using Resenas.Application.Exceptions;
using Resenas.Application.Interfaces;
using Resenas.Domain.Entities;

namespace Resenas.Application.Commands.CalificarEstudiante;

public sealed class CalificarEstudianteHandler(
    IResenaRepository resenas,
    IMarketplaceContratosClient marketplaceClient,
    ICurrentUserService currentUser)
    : IRequestHandler<CalificarEstudianteCommand, ResenaDto>
{
    public async Task<ResenaDto> Handle(
        CalificarEstudianteCommand request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al cliente autenticado.");

        if (!currentUser.EsCliente)
            throw new UnauthorizedAccessException("Solo las cuentas de cliente pueden calificar a un estudiante.");

        var autorId = currentUser.UserId;
        var contrato = await marketplaceClient.ObtenerContratoAsync(request.ContratoId, cancellationToken)
            ?? throw new KeyNotFoundException("El contrato no existe.");

        if (contrato.ClienteId != autorId)
            throw new UnauthorizedAccessException("No tienes permiso para calificar un contrato que no te pertenece.");

        if (!string.Equals(contrato.Estado, "Finalizado", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Solo puedes calificar al estudiante cuando el contrato está Finalizado.");

        if (contrato.EstudianteId == Guid.Empty)
            throw new InvalidOperationException("El contrato no tiene un estudiante válido.");

        if (await resenas.ExisteAsync(request.ContratoId, autorId, contrato.EstudianteId, cancellationToken))
            throw new ConflictException("Ya calificaste al estudiante en este contrato.");

        var resena = new Resena
        {
            Id = Guid.NewGuid(),
            ContratoId = request.ContratoId,
            AutorId = autorId,
            DestinatarioId = contrato.EstudianteId,
            Puntuacion = request.Puntuacion,
            Comentario = request.Comentario.Trim(),
            CreadaEn = DateTime.UtcNow
        };

        await resenas.AgregarAsync(resena, cancellationToken);

        return new ResenaDto(
            resena.Id,
            resena.ContratoId,
            resena.AutorId,
            resena.DestinatarioId,
            resena.Puntuacion,
            resena.Comentario,
            DateTime.SpecifyKind(resena.CreadaEn, DateTimeKind.Utc));
    }
}
