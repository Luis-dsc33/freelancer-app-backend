// ADMIN Marcos
using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using MediatR;

namespace Marketplace.Application.Queries.ListarOfertasAdmin;

public class ListarOfertasAdminHandler(IProyectoAdminRepository repositorio)
    : IRequestHandler<ListarOfertasAdminQuery, IReadOnlyList<OfertaAdminDto>>
{
    public async Task<IReadOnlyList<OfertaAdminDto>> Handle(ListarOfertasAdminQuery request, CancellationToken ct)
    {
        var ofertas = await repositorio.ListarAsync(request.Estado, ct);
        return ofertas.Select(ADto).ToList();
    }

    public static OfertaAdminDto ADto(Proyecto p) =>
        new(p.Id, p.Titulo, p.Disciplina, p.NombreCliente, p.Presupuesto, p.Estado,
            DateTime.SpecifyKind(p.CreadoEn, DateTimeKind.Utc));
}
