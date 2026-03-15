using TeamTasks.Application.Abstractions.Repositories;
using TeamTasks.Domain.Enums;

namespace TeamTasks.Application.Features.Tasks.Commands.UpdateTaskStatus;

public sealed class UpdateTaskStatusHandler
{
    private readonly ITaskRepository _taskRepository;

    public UpdateTaskStatusHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
    }

    public async Task HandleAsync(
        UpdateTaskStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.Request);

        if (command.TaskId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(command.TaskId), "TaskId must be greater than zero.");
        }

        var taskItem = await _taskRepository.GetByIdAsync(command.TaskId, cancellationToken);

        if (taskItem is null)
        {
            throw new KeyNotFoundException($"Task with id {command.TaskId} was not found.");
        }

        var newStatus = command.Request.Status;

        if (taskItem.Status == newStatus)
        {
            return;
        }

        taskItem.Status = newStatus;
        taskItem.CompletionDate = newStatus == TaskItemStatus.Completed
            ? DateTime.UtcNow
            : null;

        await _taskRepository.UpdateAsync(taskItem, cancellationToken);
        await _taskRepository.SaveChangesAsync(cancellationToken);
    }
}