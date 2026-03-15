using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTasks.Application.DTOs
{
    public sealed class DeveloperWorkloadDto
    {
        public int DeveloperId { get; init; }

        public string FullName { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public int TotalAssignedTasks { get; init; }

        public int ToDoTasks { get; init; }

        public int InProgressTasks { get; init; }

        public int BlockedTasks { get; init; }

        public int CompletedTasks { get; init; }

        public int OverdueTasks { get; init; }

        public int TotalComplexity { get; init; }
    }
}
