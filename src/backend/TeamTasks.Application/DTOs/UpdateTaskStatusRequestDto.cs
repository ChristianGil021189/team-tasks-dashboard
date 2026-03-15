using System;
using System.Collections.Generic;
using System.Text;
using TeamTasks.Domain.Enums;

namespace TeamTasks.Application.DTOs
{
    public sealed class UpdateTaskStatusRequestDto
    {
        public TaskItemStatus Status { get; set; }
    }
}
