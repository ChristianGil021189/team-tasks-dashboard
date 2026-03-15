using TeamTasks.Application.DTOs;

namespace TeamTasks.Application.Features.Tasks.Commands.UpdateTaskStatus;

public sealed record UpdateTaskStatusCommand
{
    public int TaskId { get; init; }

    public UpdateTaskStatusRequestDto Request { get; init; } = new();
}