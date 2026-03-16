using Microsoft.AspNetCore.Mvc;
using TeamTasks.Application.DTOs;
using TeamTasks.Application.Features.Tasks.Commands.CreateTask;
using TeamTasks.Application.Features.Tasks.Commands.UpdateTaskStatus;

namespace TeamTasks.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public sealed class TasksController : ControllerBase
{
    private readonly CreateTaskHandler _createTaskHandler;
    private readonly UpdateTaskStatusHandler _updateTaskStatusHandler;

    public TasksController(
        CreateTaskHandler createTaskHandler,
        UpdateTaskStatusHandler updateTaskStatusHandler)
    {
        _createTaskHandler = createTaskHandler
            ?? throw new ArgumentNullException(nameof(createTaskHandler));

        _updateTaskStatusHandler = updateTaskStatusHandler
            ?? throw new ArgumentNullException(nameof(updateTaskStatusHandler));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateTask(
        [FromBody] CreateTaskRequestDto request,
        CancellationToken cancellationToken)
    {
        var taskId = await _createTaskHandler.HandleAsync(
            new CreateTaskCommand
            {
                Request = request
            },
            cancellationToken);

        return Created($"/api/tasks/{taskId}", new { taskId });
    }

    [HttpPut("{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTaskStatus(
        int id,
        [FromBody] UpdateTaskStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest("Task id must be greater than zero.");
        }

        await _updateTaskStatusHandler.HandleAsync(
            new UpdateTaskStatusCommand
            {
                TaskId = id,
                Request = request
            },
            cancellationToken);

        return NoContent();
    }
}