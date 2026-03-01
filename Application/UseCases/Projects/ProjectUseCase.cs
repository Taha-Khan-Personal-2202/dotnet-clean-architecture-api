using Application.DTOs.Project;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.UseCases.Projects;

public sealed class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;
    private readonly ITaskService _taskService;
    private readonly IValidator<ProjectRequestDTO> _createValidator;
    private readonly IValidator<ProjectRequestUpdateDTO> _updateValidator;

    public ProjectService(
        IProjectRepository repository,
        ITaskService taskService,
        IValidator<ProjectRequestDTO> createValidator,
        IValidator<ProjectRequestUpdateDTO> updateValidator)
    {
        _repository = repository;
        _taskService = taskService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<OperationResult<ProjectResponseDTO>> CreateAsync(ProjectRequestDTO request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errors = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage));
            return OperationResult<ProjectResponseDTO>.Error(errors, 400);
        }

        var trimmedName = request.Name.Trim();
        if (await _repository.ExistsByNameAsync(trimmedName))
        {
            return OperationResult<ProjectResponseDTO>.Error(
                $"A project with name '{trimmedName}' already exists.",
                409);
        }

        var project = new Project(trimmedName, request.Description?.Trim());

        await _repository.AddAsync(project);

        return OperationResult<ProjectResponseDTO>.Ok(MapToDto(project), "Project created successfully.", 201);
    }

    public async Task<OperationResult<ProjectResponseDTO>> GetByIdAsync(Guid id)
    {
        var project = await _repository.GetByIdAsync(id);
        if (project is null)
        {
            return OperationResult<ProjectResponseDTO>.Error("Project not found.", 404);
        }

        return OperationResult<ProjectResponseDTO>.Ok(MapToDto(project));
    }

    public async Task<OperationResult<IEnumerable<ProjectResponseDTO>>> GetAllAsync()
    {
        var projects = await _repository.GetAllAsync();
        var dtos = projects.Select(MapToDto).ToList();

        return OperationResult<IEnumerable<ProjectResponseDTO>>.Ok(
            dtos,
            $"Retrieved {dtos.Count} projects.");
    }

    public async Task<OperationResult<ProjectResponseDTO>> UpdateAsync(ProjectRequestUpdateDTO request)
    {
        var validation = await _updateValidator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errors = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage));
            return OperationResult<ProjectResponseDTO>.Error(errors, 400);
        }

        var project = await _repository.GetByIdAsync(request.Id);
        if (project is null)
        {
            return OperationResult<ProjectResponseDTO>.Error("Project not found.", 404);
        }

        var newName = request.Name.Trim();

        if (!string.Equals(project.Name, newName, StringComparison.OrdinalIgnoreCase))
        {
            if (await _repository.ExistsByNameAsync(newName))
            {
                return OperationResult<ProjectResponseDTO>.Error(
                    $"Project name '{newName}' is already taken.",
                    409);
            }
        }

        // Archive check
        if (request.IsArchived && await _taskService.HasInProgressTasksForProjectAsync(project.Id))
        {
            return OperationResult<ProjectResponseDTO>.Error(
                "Cannot archive project: it has tasks in progress.",
                400);
        }

        project.Name = newName;
        project.Description = request.Description?.Trim();
        project.IsArchived = request.IsArchived;
        project.IsActive = request.IsActive;
        project.IsDeleted = request.IsDeleted;
        project.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(project);

        return OperationResult<ProjectResponseDTO>.Ok(MapToDto(project), "Project updated successfully.");
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid id)
    {
        var project = await _repository.GetByIdAsync(id);
        if (project is null)
        {
            return OperationResult<bool>.Error("Project not found.", 404);
        }

        if (await _taskService.HasInProgressTasksForProjectAsync(project.Id))
        {
            return OperationResult<bool>.Error(
                "Cannot delete project: it has tasks in progress.",
                400);
        }

        await _repository.DeleteAsync(project);

        return OperationResult<bool>.Ok(true, "Project deleted successfully.");
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _repository.ExistsByNameAsync(name.Trim());
    }

    private static ProjectResponseDTO MapToDto(Project project) => new()
    {
        Id = project.Id,
        Name = project.Name,
        Description = project.Description,
        IsArchived = project.IsArchived,
        CreatedAt = project.CreatedAt,
        UpdatedAt = project.UpdatedAt,
        IsActive = project.IsActive,
        IsDeleted = project.IsDeleted
    };
}