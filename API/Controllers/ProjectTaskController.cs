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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddAsync([FromBody] TaskRequestDTO? request)
    {
        if (request is null)
            return BadRequest(new { message = "Request body is required." });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _taskService.AddAsync(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _taskService.GetAllAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetById/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _taskService.GetByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("update/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] TaskRequestUpdateDTO? request)
    {
        if (request is null)
            return BadRequest(new { message = "Request body is required." });

        if (id != request.Id)
            return BadRequest(new { message = "ID in URL must match ID in body." });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _taskService.UpdateAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("delete/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _taskService.DeleteAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetByProjectId/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByProjectIdAsync(Guid id)
    {
        var result = await _taskService.GetByProjectIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}
