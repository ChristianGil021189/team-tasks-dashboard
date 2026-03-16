namespace TeamTasks.Application.DTOs
{
    public sealed class DeveloperDelayRiskDto
    {
        public string DeveloperName { get; init; } = string.Empty;

        public int OpenTasksCount { get; init; }

        public decimal AvgDelayDays { get; init; }

        public DateTime? NearestDueDate { get; init; }

        public DateTime? LatestDueDate { get; init; }

        public DateTime? PredictedCompletionDate { get; init; }

        public bool HighRiskFlag { get; init; }
    }
}