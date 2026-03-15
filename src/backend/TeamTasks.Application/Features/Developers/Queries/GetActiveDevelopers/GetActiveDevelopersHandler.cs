using TeamTasks.Application.Abstractions.Repositories;
using TeamTasks.Application.DTOs;

namespace TeamTasks.Application.Features.Developers.Queries.GetActiveDevelopers;

public sealed class GetActiveDevelopersHandler
{
    private readonly IDeveloperRepository _developerRepository;

    public GetActiveDevelopersHandler(IDeveloperRepository developerRepository)
    {
        _developerRepository = developerRepository ?? throw new ArgumentNullException(nameof(developerRepository));
    }

    public async Task<IReadOnlyCollection<DeveloperLookupDto>> HandleAsync(
        GetActiveDevelopersQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        return await _developerRepository.GetActiveDevelopersAsync(cancellationToken);
    }
}