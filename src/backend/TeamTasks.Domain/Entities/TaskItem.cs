using TeamTasks.Domain.Common;
using TeamTasks.Domain.Enums;

namespace TeamTasks.Domain.Entities
{
    public sealed class TaskItem : BaseEntity
    {
        public int TaskId { get; set; }

        public int ProjectId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int AssigneeId { get; set; }

        public TaskItemStatus Status { get; set; }

        public TaskPriority Priority { get; set; }

        public int EstimatedComplexity { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public Project Project { get; set; } = null!;

        public Developer Assignee { get; set; } = null!;
    }
}
