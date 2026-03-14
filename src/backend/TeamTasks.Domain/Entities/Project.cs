using System;
using System.Collections.Generic;
using System.Text;
using TeamTasks.Domain.Common;
using TeamTasks.Domain.Enums;

namespace TeamTasks.Domain.Entities
{
    public sealed class Project : BaseEntity
    {
        public int ProjectId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ClientName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ProjectStatus Status { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
