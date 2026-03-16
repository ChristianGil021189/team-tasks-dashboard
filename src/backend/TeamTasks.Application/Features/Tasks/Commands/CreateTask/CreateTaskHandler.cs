using TeamTasks.Application.Abstractions.Repositories;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enums;

namespace TeamTasks.Application.Features.Tasks.Commands.CreateTask;

public sealed class CreateTaskHandler
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IDeveloperRepository _developerRepository;

    public CreateTaskHandler(
        ITaskRepository taskRepository,
        IProjectRepository projectRepository,
        IDeveloperRepository developerRepository)
    {
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        _developerRepository = developerRepository ?? throw new ArgumentNullException(nameof(developerRepository));
    }

    public async Task<int> HandleAsync(
        CreateTaskCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(command.Request);

        var request = command.Request;

        if (request.ProjectId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.ProjectId), "ProjectId must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ArgumentException("Title is required.", nameof(request.Title));
        }

        if (request.AssigneeId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.AssigneeId), "AssigneeId must be greater than zero.");
        }

        if (request.EstimatedComplexity < 1 || request.EstimatedComplexity > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(request.EstimatedComplexity), "EstimatedComplexity must be between 1 and 5.");
        }

        if (request.DueDate == default)
        {
            throw new ArgumentException("DueDate is required.", nameof(request.DueDate));
        }

        if (!Enum.IsDefined(typeof(TaskItemStatus), request.Status))
        {
            throw new ArgumentOutOfRangeException(nameof(request.Status), "Status is invalid.");
        }

        var projectExists = await _projectRepository.ExistsAsync(request.ProjectId, cancellationToken);

        if (!projectExists)
        {
            throw new KeyNotFoundException($"Project with id {request.ProjectId} was not found.");
        }

        var developerExists = await _developerRepository.ExistsAsync(request.AssigneeId, cancellationToken);

        if (!developerExists)
        {
            throw new KeyNotFoundException($"Developer with id {request.AssigneeId} was not found.");
        }

        var developerIsActive = await _developerRepository.IsActiveAsync(request.AssigneeId, cancellationToken);

        if (!developerIsActive)
        {
            throw new InvalidOperationException($"Developer with id {request.AssigneeId} is inactive.");
        }

        var taskItem = new TaskItem
        {
            ProjectId = request.ProjectId,
            Title = request.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim(),
            AssigneeId = request.AssigneeId,
            Status = request.Status,
            CompletionDate = request.Status == TaskItemStatus.Completed? DateTime.UtcNow: null,
            Priority = request.Priority,
            EstimatedComplexity = request.EstimatedComplexity,
            DueDate = request.DueDate,
            CreatedAt = DateTime.UtcNow
        };

        await _taskRepository.AddAsync(taskItem, cancellationToken);
        await _taskRepository.SaveChangesAsync(cancellationToken);

        return taskItem.TaskId;
    }
}