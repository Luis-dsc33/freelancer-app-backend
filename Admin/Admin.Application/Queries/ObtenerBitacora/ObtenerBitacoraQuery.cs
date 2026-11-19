// ADMIN Marcos: HU-27 Consultar la bitácora administrativa (solo lectura, con filtros).
using Admin.Application.Commands.AplicarMedidaModeracion;
using Admin.Application.Dtos;
using Admin.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Admin.Application.Queries.ObtenerBitacora;

public record ObtenerBitacoraQuery(
    Guid? AdminId, string? TipoAccion, DateOnly? Desde, DateOnly? Hasta, int Pagina = 1, int TamanoPagina = 15)
    : IRequest<BitacoraDto>;

// HU-27-03 rango de fechas inválido
public class ObtenerBitacoraQueryValidator : AbstractValidator<ObtenerBitacoraQuery>
{
    public ObtenerBitacoraQueryValidator()
    {
        RuleFor(x => x)
            .Must(q => q.Desde is null || q.Hasta is null || q.Desde <= q.Hasta)
            .WithName("Fechas")
            .WithMessage("La fecha inicial no puede ser posterior a la fecha final.");
    }
}

public class ObtenerBitacoraHandler(IAccionAdministrativaRepository bitacora) : IRequestHandler<ObtenerBitacoraQuery, BitacoraDto>
{
    public async Task<BitacoraDto> Handle(ObtenerBitacoraQuery request, CancellationToken ct)
    {
        var pagina = Math.Max(1, request.Pagina);
        var tamano = Math.Clamp(request.TamanoPagina, 1, 50);
        var desde = request.Desde?.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var hasta = request.Hasta?.AddDays(1).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc); // incluye todo el día final

        var (acciones, total) = await bitacora.BuscarAsync(request.AdminId, request.TipoAccion, desde, hasta, pagina, tamano, ct);
        var administradores = await bitacora.ObtenerAdministradoresAsync(ct);

        return new BitacoraDto(acciones.Select(AplicarMedidaModeracionHandler.ADto).ToList(), total, pagina, tamano, administradores);
    }
}
