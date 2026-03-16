using System;
using System.Collections.Generic;
using System.Text;
using TeamTasks.Domain.Enums;

namespace TeamTasks.Application.DTOs
{
    public sealed class CreateTaskRequestDto
    {
        public int ProjectId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int AssigneeId { get; set; }

        public TaskItemStatus Status { get; set; }

        public TaskPriority Priority { get; set; }

        public int EstimatedComplexity { get; set; }

        public DateTime DueDate { get; set; }
    }
}
