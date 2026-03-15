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
        var today = DateTime.UtcNow.Date;

        return await _context.Developers
            .AsNoTracking()
            .Where(developer => developer.IsActive)
            .OrderBy(developer => developer.FirstName)
            .ThenBy(developer => developer.LastName)
            .Select(developer => new DeveloperWorkloadDto
            {
                DeveloperId = developer.DeveloperId,
                FullName = developer.FirstName + " " + developer.LastName,
                Email = developer.Email,
                TotalAssignedTasks = developer.AssignedTasks.Count(),
                ToDoTasks = developer.AssignedTasks.Count(task => task.Status == TaskItemStatus.ToDo),
                InProgressTasks = developer.AssignedTasks.Count(task => task.Status == TaskItemStatus.InProgress),
                BlockedTasks = developer.AssignedTasks.Count(task => task.Status == TaskItemStatus.Blocked),
                CompletedTasks = developer.AssignedTasks.Count(task => task.Status == TaskItemStatus.Completed),
                OverdueTasks = developer.AssignedTasks.Count(task =>
                    task.Status != TaskItemStatus.Completed &&
                    task.DueDate < today),
                TotalComplexity = developer.AssignedTasks.Sum(task => (int?)task.EstimatedComplexity) ?? 0
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<DeveloperDelayRiskDto>> GetDeveloperDelayRisksAsync(
        CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var dueSoonDate = today.AddDays(3);

        return await _context.Developers
            .AsNoTracking()
            .Where(developer => developer.IsActive)
            .OrderByDescending(developer => developer.AssignedTasks.Count(task =>
                task.Status != TaskItemStatus.Completed &&
                task.DueDate < today))
            .ThenByDescending(developer => developer.AssignedTasks.Count(task =>
                task.Status != TaskItemStatus.Completed &&
                task.DueDate >= today &&
                task.DueDate <= dueSoonDate))
            .ThenBy(developer => developer.FirstName)
            .ThenBy(developer => developer.LastName)
            .Select(developer => new DeveloperDelayRiskDto
            {
                DeveloperId = developer.DeveloperId,
                FullName = developer.FirstName + " " + developer.LastName,
                Email = developer.Email,
                TotalAssignedTasks = developer.AssignedTasks.Count(),
                OverdueTasks = developer.AssignedTasks.Count(task =>
                    task.Status != TaskItemStatus.Completed &&
                    task.DueDate < today),
                DueSoonTasks = developer.AssignedTasks.Count(task =>
                    task.Status != TaskItemStatus.Completed &&
                    task.DueDate >= today &&
                    task.DueDate <= dueSoonDate),
                BlockedTasks = developer.AssignedTasks.Count(task => task.Status == TaskItemStatus.Blocked),
                TotalComplexity = developer.AssignedTasks.Sum(task => (int?)task.EstimatedComplexity) ?? 0,
                DelayRiskPercentage = developer.AssignedTasks.Count() == 0
                    ? 0
                    : (
                        (
                            developer.AssignedTasks.Count(task =>
                                task.Status != TaskItemStatus.Completed &&
                                task.DueDate < today) +
                            developer.AssignedTasks.Count(task =>
                                task.Status != TaskItemStatus.Completed &&
                                task.DueDate >= today &&
                                task.DueDate <= dueSoonDate) +
                            developer.AssignedTasks.Count(task => task.Status == TaskItemStatus.Blocked)
                        ) * 100.0m
                    ) / developer.AssignedTasks.Count()
            })
            .ToListAsync(cancellationToken);
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