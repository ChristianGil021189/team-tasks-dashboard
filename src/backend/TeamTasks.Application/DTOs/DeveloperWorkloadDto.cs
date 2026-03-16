using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTasks.Application.DTOs
{
    public sealed class DeveloperWorkloadDto
    {
        public string DeveloperName { get; init; } = string.Empty;

        public int OpenTasksCount { get; init; }

        public decimal AverageEstimatedComplexity { get; init; }
    }
}
