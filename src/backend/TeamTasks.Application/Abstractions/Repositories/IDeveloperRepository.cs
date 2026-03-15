using TeamTasks.Application.DTOs;

namespace TeamTasks.Application.Abstractions.Repositories
{
    public interface IDeveloperRepository
    {
        Task<IReadOnlyCollection<DeveloperLookupDto>> GetActiveDevelopersAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<DeveloperWorkloadDto>> GetDeveloperWorkloadsAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<DeveloperDelayRiskDto>> GetDeveloperDelayRisksAsync(
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            int developerId,
            CancellationToken cancellationToken = default);

        Task<bool> IsActiveAsync(
            int developerId,
            CancellationToken cancellationToken = default);
    }
}
