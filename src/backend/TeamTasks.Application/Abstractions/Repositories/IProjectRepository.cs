using TeamTasks.Application.DTOs;
using TeamTasks.Application.Features.Projects.Queries.GetProjectTasks;

namespace TeamTasks.Application.Abstractions.Repositories
{
    public interface IProjectRepository
    {
        Task<IReadOnlyCollection<ProjectSummaryDto>> GetProjectSummariesAsync(
            CancellationToken cancellationToken = default);

        Task<PagedResultDto<ProjectTaskDto>> GetProjectTasksAsync(
            GetProjectTasksQuery query,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            int projectId,
            CancellationToken cancellationToken = default);
    }
}
