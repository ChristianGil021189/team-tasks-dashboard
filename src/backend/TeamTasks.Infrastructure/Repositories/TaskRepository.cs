using Microsoft.EntityFrameworkCore;
using TeamTasks.Application.Abstractions.Repositories;
using TeamTasks.Domain.Entities;
using TeamTasks.Infrastructure.Persistence;

namespace TeamTasks.Infrastructure.Persistence.Repositories;

public sealed class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<TaskItem?> GetByIdAsync(
        int taskId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
            .FirstOrDefaultAsync(task => task.TaskId == taskId, cancellationToken);
    }

    public async Task AddAsync(
        TaskItem taskItem,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        await _context.Tasks.AddAsync(taskItem, cancellationToken);
    }

    public Task UpdateAsync(
        TaskItem taskItem,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        _context.Tasks.Update(taskItem);

        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(
        int taskId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
            .AsNoTracking()
            .AnyAsync(task => task.TaskId == taskId, cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}