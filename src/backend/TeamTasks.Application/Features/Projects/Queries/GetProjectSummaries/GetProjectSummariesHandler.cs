using TeamTasks.Application.Abstractions.Repositories;
using TeamTasks.Application.DTOs;

namespace TeamTasks.Application.Features.Projects.Queries.GetProjectSummaries;

public sealed class GetProjectSummariesHandler
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectSummariesHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
    }

    public async Task<IReadOnlyCollection<ProjectSummaryDto>> HandleAsync(
        GetProjectSummariesQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await _projectRepository.GetProjectSummariesAsync(cancellationToken);
    }
}