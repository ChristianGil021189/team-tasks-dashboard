using System;
using System.Collections.Generic;
using System.Text;
using TeamTasks.Domain.Enums;

namespace TeamTasks.Application.DTOs
{
    public sealed class ProjectTaskDto
    {
        public int TaskId { get; init; }

        public int ProjectId { get; init; }

        public string Title { get; init; } = string.Empty;

        public string? Description { get; init; }

        public int AssigneeId { get; init; }

        public string AssigneeFullName { get; init; } = string.Empty;

        public TaskItemStatus Status { get; init; }

        public TaskPriority Priority { get; init; }

        public int EstimatedComplexity { get; init; }

        public DateTime CreatedAt { get; init; }

        public DateTime DueDate { get; init; }

        public DateTime? CompletionDate { get; init; }
    }
}
