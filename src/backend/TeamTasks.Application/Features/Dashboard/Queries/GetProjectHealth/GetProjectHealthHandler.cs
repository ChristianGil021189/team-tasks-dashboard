using TeamTasks.Application.Abstractions.Repositories;
using TeamTasks.Application.DTOs;

namespace TeamTasks.Application.Features.Dashboard.Queries.GetProjectHealth;

public sealed class GetProjectHealthHandler
{
    private readonly IDashboardRepository _dashboardRepository;

    public GetProjectHealthHandler(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository ?? throw new ArgumentNullException(nameof(dashboardRepository));
    }

    public async Task<IReadOnlyCollection<ProjectHealthDto>> HandleAsync(
        GetProjectHealthQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await _dashboardRepository.GetProjectHealthAsync(cancellationToken);
    }
}