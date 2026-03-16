using TeamTasks.Domain.Enums;

namespace TeamTasks.Application.DTOs
{
    public sealed class ProjectSummaryDto
    {
        public int ProjectId { get; init; }

        public string Name { get; init; } = string.Empty;

        public string ClientName { get; init; } = string.Empty;

        public ProjectStatus Status { get; init; }

        public DateTime StartDate { get; init; }

        public DateTime? EndDate { get; init; }

        public int TotalTasks { get; init; }

        public int OpenTasks { get; init; }

        public int CompletedTasks { get; init; }
    }
}
