using System;
using System.Collections.Generic;
using System.Text;
using TeamTasks.Domain.Common;

namespace TeamTasks.Domain.Entities
{
    public sealed class Developer : BaseEntity
    {
        public int DeveloperId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    }
}
