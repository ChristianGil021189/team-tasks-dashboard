using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTasks.Application.DTOs
{
    public sealed class PagedResultDto<T>
    {
        public IReadOnlyCollection<T> Items { get; init; } = Array.Empty<T>();

        public int Page { get; init; }

        public int PageSize { get; init; }

        public int TotalCount { get; init; }

        public int TotalPages => TotalCount <= 0
            ? 0
            : (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
