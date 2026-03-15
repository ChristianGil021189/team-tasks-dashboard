using Microsoft.EntityFrameworkCore;
using TeamTasks.Application.Abstractions.Repositories;
using TeamTasks.Application.DTOs;
using TeamTasks.Application.Features.Projects.Queries.GetProjectTasks;
using TeamTasks.Domain.Enums;
using TeamTasks.Infrastructure.Persistence;

namespace TeamTasks.Infrastructure.Persistence.Repositories;

public sealed class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IReadOnlyCollection<ProjectSummaryDto>> GetProjectSummariesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .AsNoTracking()
            .OrderBy(project => project.Name)
            .Select(project => new ProjectSummaryDto
            {
                ProjectId = project.ProjectId,
                Name = project.Name,
                ClientName = project.ClientName,
                Status = project.Status,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                TotalTasks = project.Tasks.Count(),
                CompletedTasks = project.Tasks.Count(task => task.Status == TaskItemStatus.Completed)
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResultDto<ProjectTaskDto>> GetProjectTasksAsync(
        GetProjectTasksQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var tasksQuery = _context.Tasks
            .AsNoTracking()
            .Where(task => task.ProjectId == query.ProjectId);

        if (query.Status.HasValue)
        {
            tasksQuery = tasksQuery.Where(task => task.Status == query.Status.Value);
        }

        if (query.AssigneeId.HasValue)
        {
            tasksQuery = tasksQuery.Where(task => task.AssigneeId == query.AssigneeId.Value);
        }

        var totalCount = await tasksQuery.CountAsync(cancellationToken);

        var items = await tasksQuery
            .OrderBy(task => task.DueDate)
            .ThenByDescending(task => task.Priority)
            .ThenBy(task => task.Title)
            .Select(task => new ProjectTaskDto
            {
                TaskId = task.TaskId,
                ProjectId = task.ProjectId,
                Title = task.Title,
                Description = task.Description,
                AssigneeId = task.AssigneeId,
                AssigneeFullName = $"{task.Assignee.FirstName} {task.Assignee.LastName}",
                Status = task.Status,
                Priority = task.Priority,
                EstimatedComplexity = task.EstimatedComplexity,
                DueDate = task.DueDate,
                CompletionDate = task.CompletionDate
            })
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResultDto<ProjectTaskDto>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<bool> ExistsAsync(
        int projectId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .AsNoTracking()
            .AnyAsync(project => project.ProjectId == projectId, cancellationToken);
    }
}