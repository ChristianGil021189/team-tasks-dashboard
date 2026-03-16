using Microsoft.EntityFrameworkCore;
using TeamTasks.Application.Abstractions.Repositories;
using TeamTasks.Application.DTOs;
using TeamTasks.Domain.Enums;
using TeamTasks.Infrastructure.Persistence;

namespace TeamTasks.Infrastructure.Persistence.Repositories;

public sealed class DeveloperRepository : IDeveloperRepository
{
    private readonly ApplicationDbContext _context;

    public DeveloperRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IReadOnlyCollection<DeveloperLookupDto>> GetActiveDevelopersAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Developers
            .AsNoTracking()
            .Where(developer => developer.IsActive)
            .OrderBy(developer => developer.FirstName)
            .ThenBy(developer => developer.LastName)
            .Select(developer => new DeveloperLookupDto
            {
                DeveloperId = developer.DeveloperId,
                FullName = developer.FirstName + " " + developer.LastName,
                Email = developer.Email
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<DeveloperWorkloadDto>> GetDeveloperWorkloadsAsync(
    CancellationToken cancellationToken = default)
    {
        return await _context.Developers
            .AsNoTracking()
            .Where(developer => developer.IsActive)
            .OrderBy(developer => developer.FirstName)
            .ThenBy(developer => developer.LastName)
            .Select(developer => new DeveloperWorkloadDto
            {
                DeveloperName = developer.FirstName + " " + developer.LastName,
                OpenTasksCount = developer.AssignedTasks.Count(task => task.Status != TaskItemStatus.Completed),
                AverageEstimatedComplexity =
                    developer.AssignedTasks
                        .Where(task => task.Status != TaskItemStatus.Completed)
                        .Average(task => (decimal?)task.EstimatedComplexity) ?? 0
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<DeveloperDelayRiskDto>> GetDeveloperDelayRisksAsync(
    CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;

        var data = await _context.Developers
            .AsNoTracking()
            .Where(developer => developer.IsActive)
            .Select(developer => new
            {
                DeveloperName = developer.FirstName + " " + developer.LastName,
                OpenTasksCount = developer.AssignedTasks.Count(task => task.Status != TaskItemStatus.Completed),
                AvgDelayDays = developer.AssignedTasks
                    .Where(task =>
                        task.Status != TaskItemStatus.Completed &&
                        task.DueDate < today)
                    .Average(task => (double?)EF.Functions.DateDiffDay(task.DueDate, today)) ?? 0,
                NearestDueDate = developer.AssignedTasks
                    .Where(task => task.Status != TaskItemStatus.Completed)
                    .Min(task => (DateTime?)task.DueDate),
                LatestDueDate = developer.AssignedTasks
                    .Where(task => task.Status != TaskItemStatus.Completed)
                    .Max(task => (DateTime?)task.DueDate)
            })
            .OrderByDescending(item => item.AvgDelayDays)
            .ThenByDescending(item => item.OpenTasksCount)
            .ThenBy(item => item.DeveloperName)
            .ToListAsync(cancellationToken);

        return data
            .Select(item => new DeveloperDelayRiskDto
            {
                DeveloperName = item.DeveloperName,
                OpenTasksCount = item.OpenTasksCount,
                AvgDelayDays = Convert.ToDecimal(Math.Round(item.AvgDelayDays, 2)),
                NearestDueDate = item.NearestDueDate,
                LatestDueDate = item.LatestDueDate,
                PredictedCompletionDate = item.LatestDueDate?.AddDays((int)Math.Ceiling(item.AvgDelayDays)),
                HighRiskFlag = item.OpenTasksCount > 0 &&
                               (item.AvgDelayDays >= 3 ||
                                (item.NearestDueDate.HasValue && item.NearestDueDate.Value < today))
            })
            .ToList();
    }

    public async Task<bool> ExistsAsync(
        int developerId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Developers
            .AsNoTracking()
            .AnyAsync(developer => developer.DeveloperId == developerId, cancellationToken);
    }

    public async Task<bool> IsActiveAsync(
        int developerId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Developers
            .AsNoTracking()
            .AnyAsync(developer => developer.DeveloperId == developerId && developer.IsActive, cancellationToken);
    }
}