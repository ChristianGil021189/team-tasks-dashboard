using Microsoft.AspNetCore.Mvc;
using TeamTasks.Application.DTOs;
using TeamTasks.Application.Features.Developers.Queries.GetActiveDevelopers;

namespace TeamTasks.Api.Controllers;

[ApiController]
[Route("api/developers")]
public sealed class DevelopersController : ControllerBase
{
    private readonly GetActiveDevelopersHandler _getActiveDevelopersHandler;

    public DevelopersController(GetActiveDevelopersHandler getActiveDevelopersHandler)
    {
        _getActiveDevelopersHandler = getActiveDevelopersHandler
            ?? throw new ArgumentNullException(nameof(getActiveDevelopersHandler));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<DeveloperLookupDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<DeveloperLookupDto>>> GetDevelopers(
        CancellationToken cancellationToken)
    {
        var result = await _getActiveDevelopersHandler.HandleAsync(
            new GetActiveDevelopersQuery(),
            cancellationToken);

        return Ok(result);
    }
}