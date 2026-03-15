using TeamTasks.Application.Abstractions.Repositories;
using TeamTasks.Application.DTOs;

namespace TeamTasks.Application.Features.Dashboard.Queries.GetDeveloperWorkloads;

public sealed class GetDeveloperWorkloadsHandler
{
    private readonly IDeveloperRepository _developerRepository;

    public GetDeveloperWorkloadsHandler(IDeveloperRepository developerRepository)
    {
        _developerRepository = developerRepository ?? throw new ArgumentNullException(nameof(developerRepository));
    }

    public async Task<IReadOnlyCollection<DeveloperWorkloadDto>> HandleAsync(
        GetDeveloperWorkloadsQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await _developerRepository.GetDeveloperWorkloadsAsync(cancellationToken);
    }
}