using TeamTasks.Application.Abstractions.Repositories;
using TeamTasks.Application.DTOs;

namespace TeamTasks.Application.Features.Dashboard.Queries.GetDeveloperDelayRisks;

public sealed class GetDeveloperDelayRisksHandler
{
    private readonly IDeveloperRepository _developerRepository;

    public GetDeveloperDelayRisksHandler(IDeveloperRepository developerRepository)
    {
        _developerRepository = developerRepository ?? throw new ArgumentNullException(nameof(developerRepository));
    }

    public async Task<IReadOnlyCollection<DeveloperDelayRiskDto>> HandleAsync(
        GetDeveloperDelayRisksQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await _developerRepository.GetDeveloperDelayRisksAsync(cancellationToken);
    }
}