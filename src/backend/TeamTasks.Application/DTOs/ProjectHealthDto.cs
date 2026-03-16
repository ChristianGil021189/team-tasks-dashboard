using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTasks.Application.DTOs
{
    public sealed class ProjectHealthDto
    {
        public int ProjectId { get; init; }

        public string ProjectName { get; init; } = string.Empty;

        public string ClientName { get; init; } = string.Empty;

        public int TotalTasks { get; init; }

        public int OpenTasks { get; init; }

        public int CompletedTasks { get; init; }

        public int InProgressTasks { get; init; }

        public int BlockedTasks { get; init; }

        public int OverdueTasks { get; init; }

        public decimal CompletionPercentage { get; init; }

        public decimal RiskPercentage { get; init; }
    }
}
