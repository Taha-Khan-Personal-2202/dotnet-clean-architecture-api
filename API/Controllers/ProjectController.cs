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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] ProjectRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.AddAsync(request);
        return Ok(result);
    }

    [HttpGet("GetById/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var project = await _service.GetByIdAsync(id);
        return project is null ? NotFound() : Ok(project);
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll() 
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpPut("update/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProjectRequestUpdateDTO request)
    {
        if (id != request.Id)
            return BadRequest("ID in URL must match ID in body");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(await _service.UpdateAsync(request));
    }

    [HttpDelete("delete/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpHead("exists/name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExistsByName(string name)
    {
        var exists = await _service.ExistsByNameAsync(name);
        return exists ? Ok() : NotFound();
    }
}