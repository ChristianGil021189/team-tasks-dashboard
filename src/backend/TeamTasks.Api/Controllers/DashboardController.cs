using Microsoft.AspNetCore.Mvc;
using TeamTasks.Application.DTOs;
using TeamTasks.Application.Features.Dashboard.Queries.GetDeveloperDelayRisks;
using TeamTasks.Application.Features.Dashboard.Queries.GetProjectHealth;
using TeamTasks.Application.Features.Dashboard.Queries.GetDeveloperWorkloads;

namespace TeamTasks.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public sealed class DashboardController : ControllerBase
{
    private readonly GetDeveloperWorkloadsHandler _getDeveloperWorkloadsHandler;
    private readonly GetProjectHealthHandler _getProjectHealthHandler;
    private readonly GetDeveloperDelayRisksHandler _getDeveloperDelayRisksHandler;

    public DashboardController(
        GetDeveloperWorkloadsHandler getDeveloperWorkloadsHandler,
        GetProjectHealthHandler getProjectHealthHandler,
        GetDeveloperDelayRisksHandler getDeveloperDelayRisksHandler)
    {
        _getDeveloperWorkloadsHandler = getDeveloperWorkloadsHandler
            ?? throw new ArgumentNullException(nameof(getDeveloperWorkloadsHandler));

        _getProjectHealthHandler = getProjectHealthHandler
            ?? throw new ArgumentNullException(nameof(getProjectHealthHandler));

        _getDeveloperDelayRisksHandler = getDeveloperDelayRisksHandler
            ?? throw new ArgumentNullException(nameof(getDeveloperDelayRisksHandler));
    }

    [HttpGet("developer-workload")]
    [ProducesResponseType(typeof(IReadOnlyCollection<DeveloperWorkloadDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<DeveloperWorkloadDto>>> GetDeveloperWorkload(
        CancellationToken cancellationToken)
    {
        var result = await _getDeveloperWorkloadsHandler.HandleAsync(
            new GetDeveloperWorkloadsQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("project-health")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ProjectHealthDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ProjectHealthDto>>> GetProjectHealth(
        CancellationToken cancellationToken)
    {
        var result = await _getProjectHealthHandler.HandleAsync(
            new GetProjectHealthQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("developer-delay-risk")]
    [ProducesResponseType(typeof(IReadOnlyCollection<DeveloperDelayRiskDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<DeveloperDelayRiskDto>>> GetDeveloperDelayRisk(
        CancellationToken cancellationToken)
    {
        var result = await _getDeveloperDelayRisksHandler.HandleAsync(
            new GetDeveloperDelayRisksQuery(),
            cancellationToken);

        return Ok(result);
    }
}