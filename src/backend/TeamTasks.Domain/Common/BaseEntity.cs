using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTasks.Domain.Common
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; }
    }
}
