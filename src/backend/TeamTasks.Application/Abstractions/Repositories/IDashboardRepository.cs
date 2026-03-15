using System;
using System.Collections.Generic;
using System.Text;
using TeamTasks.Application.DTOs;

namespace TeamTasks.Application.Abstractions.Repositories
{
    public interface IDashboardRepository
    {
        Task<IReadOnlyCollection<ProjectHealthDto>> GetProjectHealthAsync(CancellationToken cancellationToken = default);
    }
}
