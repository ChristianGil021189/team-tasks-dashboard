using TeamTasks.Domain.Enums;

namespace TeamTasks.Application.Features.Projects.Queries.GetProjectTasks;

public sealed record GetProjectTasksQuery
{
    public int ProjectId { get; init; }

    public TaskItemStatus? Status { get; init; }

    public int? AssigneeId { get; init; }

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 10;
}