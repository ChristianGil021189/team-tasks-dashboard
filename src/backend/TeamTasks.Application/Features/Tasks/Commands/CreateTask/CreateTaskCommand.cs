using TeamTasks.Application.DTOs;

namespace TeamTasks.Application.Features.Tasks.Commands.CreateTask;

public sealed record CreateTaskCommand
{
    public CreateTaskRequestDto Request { get; init; } = new();
}