using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTasks.Application.DTOs
{
    public sealed class DeveloperDelayRiskDto
    {
        public int DeveloperId { get; init; }

        public string FullName { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public int TotalAssignedTasks { get; init; }

        public int OverdueTasks { get; init; }

        public int DueSoonTasks { get; init; }

        public int BlockedTasks { get; init; }

        public int TotalComplexity { get; init; }

        public decimal DelayRiskPercentage { get; init; }
    }
}
