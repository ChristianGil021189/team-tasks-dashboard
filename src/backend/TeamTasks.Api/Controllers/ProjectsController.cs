using Microsoft.AspNetCore.Mvc;
using TeamTasks.Application.DTOs;
using TeamTasks.Application.Features.Projects.Queries.GetProjectSummaries;
using TeamTasks.Application.Features.Projects.Queries.GetProjectTasks;
using TeamTasks.Domain.Enums;

namespace TeamTasks.Api.Controllers;

[ApiController]
[Route("api/projects")]
public sealed class ProjectsController : ControllerBase
{
    private readonly GetProjectSummariesHandler _getProjectSummariesHandler;
    private readonly GetProjectTasksHandler _getProjectTasksHandler;

    public ProjectsController(
        GetProjectSummariesHandler getProjectSummariesHandler,
        GetProjectTasksHandler getProjectTasksHandler)
    {
        _getProjectSummariesHandler = getProjectSummariesHandler
            ?? throw new ArgumentNullException(nameof(getProjectSummariesHandler));

        _getProjectTasksHandler = getProjectTasksHandler
            ?? throw new ArgumentNullException(nameof(getProjectTasksHandler));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ProjectSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ProjectSummaryDto>>> GetProjects(
        CancellationToken cancellationToken)
    {
        var result = await _getProjectSummariesHandler.HandleAsync(
            new GetProjectSummariesQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}/tasks")]
    [ProducesResponseType(typeof(PagedResultDto<ProjectTaskDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResultDto<ProjectTaskDto>>> GetProjectTasks(
        int id,
        [FromQuery] TaskItemStatus? status,
        [FromQuery] int? assigneeId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            return BadRequest("Project id must be greater than zero.");
        }

        if (page <= 0)
        {
            return BadRequest("Page must be greater than zero.");
        }

        if (pageSize <= 0)
        {
            return BadRequest("PageSize must be greater than zero.");
        }

        var query = new GetProjectTasksQuery
        {
            ProjectId = id,
            Status = status,
            AssigneeId = assigneeId,
            Page = page,
            PageSize = pageSize
        };

        var result = await _getProjectTasksHandler.HandleAsync(query, cancellationToken);

        return Ok(result);
    }
}