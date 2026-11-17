using MediatR;
using Marketplace.Application.Interfaces;
using Marketplace.Application.Queries.Contratos;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Commands.CrearContrato;

public sealed class CrearContratoHandler(
    IPostulacionRepository postulaciones,
    IContratoRepository contratos,
    ICurrentUserService currentUser)
    : IRequestHandler<CrearContratoCommand, ContratoDto>
{
    public async Task<ContratoDto> Handle(CrearContratoCommand request, CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al cliente autenticado.");

        var postulacion = await postulaciones.ObtenerPorIdAsync(request.PostulacionId, cancellationToken)
            ?? throw new KeyNotFoundException("La postulación no existe.");
        if (postulacion.Proyecto.ClienteId != currentUser.UserId)
            throw new UnauthorizedAccessException("La postulación no pertenece al cliente autenticado.");
        if (postulacion.Estado != Postulacion.EstadoAceptada)
            throw new InvalidOperationException("Solo se puede contratar una postulación aceptada.");
        if (await contratos.ExisteParaPostulacionAsync(request.PostulacionId, cancellationToken))
            throw new InvalidOperationException("La postulación ya tiene un contrato.");

        var contrato = new Contrato
        {
            Id = Guid.NewGuid(),
            PostulacionId = postulacion.Id,
            ProyectoId = postulacion.ProyectoId,
            ClienteId = currentUser.UserId,
            EstudianteId = postulacion.EstudianteId,
            ImporteAcordado = request.ImporteAcordado,
            FechaInicio = request.FechaInicio,
            FechaCompromiso = request.FechaCompromiso,
            CreadoEn = DateTime.UtcNow
        };
        await contratos.AgregarAsync(contrato, cancellationToken);
        return ContratoMapper.ToDto(contrato);
    }
}
