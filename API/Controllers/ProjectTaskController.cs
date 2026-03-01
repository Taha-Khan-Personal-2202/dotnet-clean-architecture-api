using Application.DTOs.Task;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/project-tasks")]
[ApiController]
public class ProjectTasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public ProjectTasksController(ITaskService taskService)
    {
        _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
    }

    [HttpPost]
    [ProducesResponseType(typeof(OperationResult<TaskResponseDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(OperationResult<TaskResponseDTO>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(OperationResult<TaskResponseDTO>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] TaskRequestDTO request)
    {
        var result = await _taskService.AddAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(OperationResult<IEnumerable<TaskResponseDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _taskService.GetAllAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OperationResult<TaskResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(OperationResult<TaskResponseDTO>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _taskService.GetByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(OperationResult<TaskResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(OperationResult<TaskResponseDTO>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(OperationResult<TaskResponseDTO>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] TaskRequestUpdateDTO request)
    {
        if (id != request.Id)
            return BadRequest("ID in URL must match ID in body");

        var result = await _taskService.UpdateAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(OperationResult<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _taskService.DeleteAsync(id);
        if (!result.Success)
            return StatusCode(result.StatusCode, result.Message);

        return NoContent();
    }

    [HttpGet("project/{projectId:guid}")]
    [ProducesResponseType(typeof(OperationResult<IEnumerable<TaskResponseDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(OperationResult<IEnumerable<TaskResponseDTO>>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByProjectId(Guid projectId)
    {
        var result = await _taskService.GetByProjectIdAsync(projectId);
        return StatusCode(result.StatusCode, result);
    }
}