using MediatR;
using Resenas.Application.Dtos;
using Resenas.Application.Exceptions;
using Resenas.Application.Interfaces;
using Resenas.Domain.Entities;

namespace Resenas.Application.Commands.CalificarCliente;

public sealed class CalificarClienteHandler(
    IResenaRepository resenas,
    IMarketplaceContratosClient marketplaceClient,
    ICurrentUserService currentUser)
    : IRequestHandler<CalificarClienteCommand, ResenaDto>
{
    public async Task<ResenaDto> Handle(
        CalificarClienteCommand request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al estudiante autenticado.");

        if (!currentUser.EsEstudiante)
            throw new UnauthorizedAccessException("Solo las cuentas de estudiante pueden calificar a un cliente.");

        var autorId = currentUser.UserId;
        var contrato = await marketplaceClient.ObtenerContratoAsync(request.ContratoId, cancellationToken)
            ?? throw new KeyNotFoundException("El contrato no existe.");

        if (contrato.EstudianteId != autorId)
            throw new UnauthorizedAccessException("No tienes permiso para calificar un contrato que no te pertenece.");

        if (!string.Equals(contrato.Estado, "Finalizado", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Solo puedes calificar al cliente cuando el contrato está Finalizado.");

        if (await resenas.ExisteAsync(request.ContratoId, autorId, contrato.ClienteId, cancellationToken))
            throw new ConflictException("Ya calificaste al cliente en este contrato.");

        var resena = new Resena
        {
            Id = Guid.NewGuid(),
            ContratoId = request.ContratoId,
            AutorId = autorId,
            DestinatarioId = contrato.ClienteId,
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
