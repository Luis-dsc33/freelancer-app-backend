// ADMIN Marcos: primero se aplica la medida en el servicio dueño y SOLO si confirma se registra en la bitácora
// (HU-21-04). Admin nunca modifica tablas de otros schemas.
using Admin.Application.Dtos;
using Admin.Application.Interfaces;
using Admin.Domain.Entities;
using MediatR;

namespace Admin.Application.Commands.AplicarMedidaModeracion;

public class AplicarMedidaModeracionHandler(
    IUsuariosAdminClient usuarios,
    IMarketplaceAdminClient marketplace,
    IAccionAdministrativaRepository bitacora,
    IUsuarioActual admin)
    : IRequestHandler<AplicarMedidaModeracionCommand, AccionDto>
{
    public async Task<AccionDto> Handle(AplicarMedidaModeracionCommand request, CancellationToken ct)
    {
        var (recursoTipo, recursoNombre) = request.TipoAccion switch
        {
            AccionAdministrativa.Suspender => DeUsuario(await usuarios.SuspenderAsync(request.RecursoId, request.Motivo.Trim(), ct)),
            AccionAdministrativa.Reactivar => DeUsuario(await usuarios.ReactivarAsync(request.RecursoId, ct)),
            AccionAdministrativa.Ocultar => DeOferta(await marketplace.OcultarAsync(request.RecursoId, ct)),
            AccionAdministrativa.Restaurar => DeOferta(await marketplace.RestaurarAsync(request.RecursoId, ct)),
            _ => throw new InvalidOperationException("Tipo de medida no válido.")
        };

        var accion = new AccionAdministrativa
        {
            Id = Guid.NewGuid(),
            AdminId = admin.Id,
            AdminNombre = admin.Nombre,
            TipoAccion = request.TipoAccion,
            RecursoTipo = recursoTipo,
            RecursoId = request.RecursoId,
            RecursoNombre = recursoNombre,
            Motivo = request.Motivo.Trim(),
            Fecha = DateTime.UtcNow
        };
        await bitacora.AgregarAsync(accion, ct);
        return ADto(accion);
    }

    private static (string, string) DeUsuario(UsuarioDto u) => (AccionAdministrativa.RecursoUsuario, $"{u.Nombre} ({u.Email})");
    private static (string, string) DeOferta(OfertaDto o) => (AccionAdministrativa.RecursoOferta, o.Titulo);

    public static AccionDto ADto(AccionAdministrativa a) =>
        new(a.Id, a.AdminId, a.AdminNombre, a.TipoAccion, a.RecursoTipo, a.RecursoId, a.RecursoNombre, a.Motivo,
            DateTime.SpecifyKind(a.Fecha, DateTimeKind.Utc));
}
