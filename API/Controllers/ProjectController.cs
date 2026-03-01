using Application.DTOs.Project;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/projects")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _service;

    public ProjectsController(IProjectService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(OperationResult<ProjectResponseDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(OperationResult<ProjectResponseDTO>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(OperationResult<ProjectResponseDTO>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateProject([FromBody] ProjectRequestDTO request)
    {
        var result = await _service.CreateAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OperationResult<ProjectResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(OperationResult<ProjectResponseDTO>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(OperationResult<IEnumerable<ProjectResponseDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(OperationResult<ProjectResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(OperationResult<ProjectResponseDTO>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(OperationResult<ProjectResponseDTO>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProjectRequestUpdateDTO request)
    {
        if (id != request.Id)
            return BadRequest("ID in URL must match ID in body");

        var result = await _service.UpdateAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result.Success)
            return StatusCode(result.StatusCode, result.Message);

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