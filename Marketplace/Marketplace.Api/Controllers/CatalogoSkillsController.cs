using Marketplace.Application.Queries.CatalogoSkills;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
[Route("api/catalogo/habilidades")]
[Authorize]
public sealed class CatalogoSkillsController(IMediator mediator) : ControllerBase
{
    [HttpGet("categorias")]
    public Task<IReadOnlyList<SkillCategoryDto>> Categorias(CancellationToken cancellationToken) =>
        mediator.Send(new ListarSkillCategoriesQuery(), cancellationToken);

    [HttpGet]
    public Task<IReadOnlyList<SkillDto>> Skills([FromQuery] Guid? categoriaId, CancellationToken cancellationToken) =>
        mediator.Send(new ListarSkillsQuery(categoriaId), cancellationToken);
}
