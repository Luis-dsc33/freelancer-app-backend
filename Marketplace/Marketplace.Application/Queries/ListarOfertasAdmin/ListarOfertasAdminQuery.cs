// ADMIN Marcos: listado de ofertas para moderación (incluye las ocultas).
using MediatR;

namespace Marketplace.Application.Queries.ListarOfertasAdmin;

public record ListarOfertasAdminQuery(string? Estado) : IRequest<IReadOnlyList<OfertaAdminDto>>;

public record OfertaAdminDto(
    Guid Id, string Titulo, string Disciplina, string NombreCliente, decimal Presupuesto, string Estado, DateTime CreadoEn);
