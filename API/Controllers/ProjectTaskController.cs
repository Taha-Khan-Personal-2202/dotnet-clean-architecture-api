using Application.DTOs.Task;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectTaskController(ITaskService service) : ControllerBase
{
    private readonly ITaskService _taskService = service;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddAsync([FromBody] TaskRequestDTO request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var response = await _taskService.AddAsync(request);
        return Ok(response);
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync()
    {
        var tasks = await _taskService.GetAllAsync();
        return tasks == null ? NotFound() : Ok(tasks);
    }

    [HttpGet("GetById/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var task = await _taskService.GetByIdAsync(id);
        return task == null ? NotFound() : Ok(task);
    }

    [HttpPut("update/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] TaskRequestUpdateDTO request)
    {
        if (id != request.Id) throw new InvalidOperationException("ID in URL must match ID in body");
        ArgumentNullException.ThrowIfNull(request);
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var task = await _taskService.UpdateAsync(request);
        return Ok(task);
    }

    [HttpDelete("delete/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _taskService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("GetByProjectId/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByProjectIdAsync(Guid id)
    {
        var tasks = await _taskService.GetByProjectIdAsync(id);
        return tasks != null && tasks.Count != 0 ? Ok(tasks) : NotFound();
    }

}
