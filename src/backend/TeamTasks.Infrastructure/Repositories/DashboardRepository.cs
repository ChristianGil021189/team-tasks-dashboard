using Microsoft.EntityFrameworkCore;
using TeamTasks.Application.Abstractions.Repositories;
using TeamTasks.Application.DTOs;
using TeamTasks.Domain.Enums;
using TeamTasks.Infrastructure.Persistence;

namespace TeamTasks.Infrastructure.Persistence.Repositories;

public sealed class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDbContext _context;

    public DashboardRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IReadOnlyCollection<ProjectHealthDto>> GetProjectHealthAsync(
    CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;

        return await _context.Projects
            .AsNoTracking()
            .OrderBy(project => project.Name)
            .Select(project => new ProjectHealthDto
            {
                ProjectId = project.ProjectId,
                ProjectName = project.Name,
                ClientName = project.ClientName,
                TotalTasks = project.Tasks.Count(),
                OpenTasks = project.Tasks.Count(task => task.Status != TaskItemStatus.Completed),
                CompletedTasks = project.Tasks.Count(task => task.Status == TaskItemStatus.Completed),
                InProgressTasks = project.Tasks.Count(task => task.Status == TaskItemStatus.InProgress),
                BlockedTasks = project.Tasks.Count(task => task.Status == TaskItemStatus.Blocked),
                OverdueTasks = project.Tasks.Count(task =>
                    task.Status != TaskItemStatus.Completed &&
                    task.DueDate < today),
                CompletionPercentage = project.Tasks.Count() == 0
                    ? 0
                    : (project.Tasks.Count(task => task.Status == TaskItemStatus.Completed) * 100.0m) / project.Tasks.Count(),
                RiskPercentage = project.Tasks.Count() == 0
                    ? 0
                    : (
                        (
                            project.Tasks.Count(task => task.Status == TaskItemStatus.Blocked) +
                            project.Tasks.Count(task =>
                                task.Status != TaskItemStatus.Completed &&
                                task.DueDate < today)
                        ) * 100.0m
                    ) / project.Tasks.Count()
            })
            .ToListAsync(cancellationToken);
    }
}