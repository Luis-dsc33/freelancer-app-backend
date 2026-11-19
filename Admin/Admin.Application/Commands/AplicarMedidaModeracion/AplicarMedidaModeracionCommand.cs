// ADMIN Marcos: HU-21 (suspender / ocultar) y HU-26 (reactivar / restaurar). Una sola acción con su tipo.
using Admin.Application.Dtos;
using MediatR;

namespace Admin.Application.Commands.AplicarMedidaModeracion;

public record AplicarMedidaModeracionCommand(string TipoAccion, Guid RecursoId, string Motivo) : IRequest<AccionDto>;
