using TeamTasks.Domain.Entities;

namespace TeamTasks.Application.Abstractions.Repositories
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(int taskId, CancellationToken cancellationToken = default);

        Task AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default);

        Task UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(int taskId, CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
