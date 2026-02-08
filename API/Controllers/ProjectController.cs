using Application.DTOs.Project;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController(IProjectService service) : ControllerBase
{
    private readonly IProjectService _service = service;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] ProjectRequestDTO? request)
    {
        if (request is null)
            return BadRequest(new { message = "Request body is required." });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.AddAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetById/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("update/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProjectRequestUpdateDTO? request)
    {
        if (request is null)
            return BadRequest(new { message = "Request body is required." });

        if (id != request.Id)
            return BadRequest(new { message = "ID in URL must match ID in body." });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.UpdateAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("delete/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpHead("exists/name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExistsByName(string name)
    {
        var result = await _service.ExistsByNameAsync(name);
        return result.IsSuccess ? Ok() : NotFound(new { message = result.Message });
    }
}
