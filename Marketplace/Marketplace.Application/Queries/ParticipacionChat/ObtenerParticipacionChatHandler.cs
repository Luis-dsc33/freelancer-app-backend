using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using MediatR;

namespace Marketplace.Application.Queries.ParticipacionChat;

public sealed class ObtenerParticipacionChatHandler(
    IProyectoRepository proyectos,
    IPostulacionRepository postulaciones,
    IContratoRepository contratos,
    ICurrentUserService currentUser)
    : IRequestHandler<ObtenerParticipacionChatQuery, ParticipacionChatDto>
{
    public async Task<ParticipacionChatDto> Handle(
        ObtenerParticipacionChatQuery request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId == Guid.Empty)
            throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado.");

        var proyecto = await proyectos.ObtenerPorIdAsync(request.ProyectoId, cancellationToken)
            ?? throw new KeyNotFoundException("La oferta no existe.");

        if (currentUser.EsEstudiante)
        {
            return await ResolverParaEstudianteAsync(proyecto, request, currentUser.UserId, cancellationToken);
        }

        if (currentUser.EsCliente)
        {
            return await ResolverParaClienteAsync(proyecto, request, currentUser.UserId, cancellationToken);
        }

        return NoAutorizado(proyecto);
    }

    private async Task<ParticipacionChatDto> ResolverParaEstudianteAsync(
        Proyecto proyecto,
        ObtenerParticipacionChatQuery request,
        Guid estudianteId,
        CancellationToken ct)
    {
        if (request.ContratoId.HasValue)
        {
            var contrato = await contratos.ObtenerPorIdAsync(request.ContratoId.Value, ct);
            if (contrato is null ||
                contrato.ProyectoId != proyecto.Id ||
                contrato.EstudianteId != estudianteId ||
                contrato.Estado is not (Contrato.EstadoActivo or Contrato.EstadoFinalizado))
            {
                return NoAutorizado(proyecto);
            }

            return new ParticipacionChatDto(
                true,
                proyecto.ClienteId,
                estudianteId,
                proyecto.Titulo,
                proyecto.NombreCliente,
                contrato.PostulacionId,
                contrato.Id);
        }

        if (request.PostulacionId.HasValue)
        {
            var postulacion = await postulaciones.ObtenerPorIdAsync(request.PostulacionId.Value, ct);
            if (postulacion is null ||
                postulacion.ProyectoId != proyecto.Id ||
                postulacion.EstudianteId != estudianteId ||
                postulacion.Estado is Postulacion.EstadoRechazada or Postulacion.EstadoRetirada)
            {
                return NoAutorizado(proyecto);
            }

            var contrato = await contratos.ObtenerParaEstudianteEnProyectoAsync(proyecto.Id, estudianteId, ct);
            var contratoValido = contrato is not null && contrato.Estado is Contrato.EstadoActivo or Contrato.EstadoFinalizado
                ? contrato.Id
                : (Guid?)null;

            return new ParticipacionChatDto(
                true,
                proyecto.ClienteId,
                estudianteId,
                proyecto.Titulo,
                proyecto.NombreCliente,
                postulacion.Id,
                contratoValido);
        }

        var contratoExistente = await contratos.ObtenerParaEstudianteEnProyectoAsync(proyecto.Id, estudianteId, ct);
        if (contratoExistente is not null && contratoExistente.Estado is Contrato.EstadoActivo or Contrato.EstadoFinalizado)
        {
            return new ParticipacionChatDto(
                true,
                proyecto.ClienteId,
                estudianteId,
                proyecto.Titulo,
                proyecto.NombreCliente,
                contratoExistente.PostulacionId,
                contratoExistente.Id);
        }

        var postulacionExistente = await postulaciones.ObtenerParaEstudianteEnProyectoAsync(proyecto.Id, estudianteId, ct);
        if (postulacionExistente is not null &&
            postulacionExistente.Estado is not (Postulacion.EstadoRechazada or Postulacion.EstadoRetirada))
        {
            return new ParticipacionChatDto(
                true,
                proyecto.ClienteId,
                estudianteId,
                proyecto.Titulo,
                proyecto.NombreCliente,
                postulacionExistente.Id,
                null);
        }

        return NoAutorizado(proyecto);
    }

    private async Task<ParticipacionChatDto> ResolverParaClienteAsync(
        Proyecto proyecto,
        ObtenerParticipacionChatQuery request,
        Guid clienteId,
        CancellationToken ct)
    {
        if (proyecto.ClienteId != clienteId)
            return NoAutorizado(proyecto);

        if (request.ContratoId.HasValue)
        {
            var contrato = await contratos.ObtenerPorIdAsync(request.ContratoId.Value, ct);
            if (contrato is null ||
                contrato.ProyectoId != proyecto.Id ||
                contrato.ClienteId != clienteId ||
                contrato.Estado is not (Contrato.EstadoActivo or Contrato.EstadoFinalizado))
            {
                return NoAutorizado(proyecto);
            }

            return new ParticipacionChatDto(
                true,
                clienteId,
                contrato.EstudianteId,
                proyecto.Titulo,
                proyecto.NombreCliente,
                contrato.PostulacionId,
                contrato.Id);
        }

        if (request.PostulacionId.HasValue)
        {
            var postulacion = await postulaciones.ObtenerPorIdAsync(request.PostulacionId.Value, ct);
            if (postulacion is null ||
                postulacion.ProyectoId != proyecto.Id ||
                postulacion.Estado is Postulacion.EstadoRechazada or Postulacion.EstadoRetirada)
            {
                return NoAutorizado(proyecto);
            }

            return new ParticipacionChatDto(
                true,
                clienteId,
                postulacion.EstudianteId,
                proyecto.Titulo,
                proyecto.NombreCliente,
                postulacion.Id,
                null);
        }

        return NoAutorizado(proyecto);
    }

    private static ParticipacionChatDto NoAutorizado(Proyecto proyecto) =>
        new(false, proyecto.ClienteId, Guid.Empty, proyecto.Titulo, proyecto.NombreCliente, null, null);
}
