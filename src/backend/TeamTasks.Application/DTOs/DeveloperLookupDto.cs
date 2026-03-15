namespace TeamTasks.Application.DTOs;

public sealed class DeveloperLookupDto
{
    public int DeveloperId { get; init; }

    public string FullName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;
}