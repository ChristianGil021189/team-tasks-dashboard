using TeamTasks.Application.Abstractions.Repositories;
using TeamTasks.Application.DTOs;

namespace TeamTasks.Application.Features.Projects.Queries.GetProjectTasks;

public sealed class GetProjectTasksHandler
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectTasksHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
    }

    public async Task<PagedResultDto<ProjectTaskDto>> HandleAsync(
        GetProjectTasksQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (query.ProjectId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(query.ProjectId), "ProjectId must be greater than zero.");
        }

        if (query.Page <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(query.Page), "Page must be greater than zero.");
        }

        if (query.PageSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(query.PageSize), "PageSize must be greater than zero.");
        }

        var projectExists = await _projectRepository.ExistsAsync(query.ProjectId, cancellationToken);

        if (!projectExists)
        {
            throw new KeyNotFoundException($"Project with id {query.ProjectId} was not found.");
        }

        return await _projectRepository.GetProjectTasksAsync(query, cancellationToken);
    }
}