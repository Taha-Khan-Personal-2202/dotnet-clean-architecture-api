using Application.Common.Responses;
using Application.DTOs.Project;
using Application.Interfaces;
using Application.Validators.Projects;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Projects;

public class ProjectUseCase(
    IProjectRepository repository,
    ProjectValidator createValidations,
    UpdateProjectValidator updateValidations,
    ITaskService taskService) : IProjectService
{
    private readonly IProjectRepository _repository = repository;
    private readonly ITaskService _taskService = taskService;
    private readonly ProjectValidator _createValidations = createValidations;
    private readonly UpdateProjectValidator _updateValidations = updateValidations;

    public async Task<OperationResult<ProjectResponseDTO>> AddAsync(ProjectRequestDTO request)
    {
        var validationResult = await _createValidations.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return OperationResult<ProjectResponseDTO>.Fail($"Validation failed: {errors}", 400);
        }

        if (await _repository.ExistsByNameAsync(request.Name.Trim()))
            return OperationResult<ProjectResponseDTO>.Fail($"Project with name '{request.Name}' already exists.", 409);

        var project = new Project(request.Name.Trim(), request.Description?.Trim());
        await _repository.AddAsync(project);

        return OperationResult<ProjectResponseDTO>.Success(MapEntityToDTO(project), "Project created successfully.", 201);
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid id)
    {
        var project = await _repository.GetByIdAsync(id);
        if (project is null)
            return OperationResult<bool>.Fail($"Project with ID {id} not found.", 404);

        var hasInProgressTasks = await _taskService.FindInProgressTasksAsync();
        if (!hasInProgressTasks.IsSuccess)
            return OperationResult<bool>.Fail(hasInProgressTasks.Message, hasInProgressTasks.StatusCode);

        if (hasInProgressTasks.Data)
            return OperationResult<bool>.Fail("The project cannot be archived because it has tasks in progress.", 409);

        await _repository.DeleteAsync(project);
        return OperationResult<bool>.Success(true, "Project deleted successfully.");
    }

    public async Task<OperationResult<bool>> ExistsByNameAsync(string name)
    {
        var exists = await _repository.ExistsByNameAsync(name);
        return exists
            ? OperationResult<bool>.Success(true, $"Project '{name}' exists.")
            : OperationResult<bool>.Fail($"Project '{name}' was not found.", 404);
    }

    public async Task<OperationResult<List<ProjectResponseDTO>>> GetAllAsync()
    {
        var projects = await _repository.GetAllAsync();
        var responseDTOs = projects.Select(MapEntityToDTO).ToList();
        return OperationResult<List<ProjectResponseDTO>>.Success(responseDTOs, "Projects fetched successfully.");
    }

    public async Task<OperationResult<ProjectResponseDTO>> GetByIdAsync(Guid id)
    {
        var project = await _repository.GetByIdAsync(id);
        if (project is null)
            return OperationResult<ProjectResponseDTO>.Fail($"Project with ID {id} not found.", 404);

        return OperationResult<ProjectResponseDTO>.Success(MapEntityToDTO(project), "Project fetched successfully.");
    }

    public async Task<OperationResult<ProjectResponseDTO>> UpdateAsync(ProjectRequestUpdateDTO request)
    {
        var validationResult = await _updateValidations.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return OperationResult<ProjectResponseDTO>.Fail($"Validation failed: {errors}", 400);
        }

        var project = await _repository.GetByIdAsync(request.Id);
        if (project is null)
            return OperationResult<ProjectResponseDTO>.Fail($"Project with ID {request.Id} not found.", 404);

        if (!string.Equals(project.Name, request.Name, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByNameAsync(request.Name))
        {
            return OperationResult<ProjectResponseDTO>.Fail($"Project name '{request.Name}' is already taken.", 409);
        }

        var hasInProgressTasks = await _taskService.FindInProgressTasksAsync();
        if (!hasInProgressTasks.IsSuccess)
            return OperationResult<ProjectResponseDTO>.Fail(hasInProgressTasks.Message, hasInProgressTasks.StatusCode);

        if (request.IsArchived && hasInProgressTasks.Data)
            return OperationResult<ProjectResponseDTO>.Fail("The project cannot be archived because it has tasks in progress.", 409);

        project.Name = request.Name.Trim();
        project.Description = request.Description?.Trim();
        project.IsArchived = request.IsArchived;
        project.UpdatedAt = DateTime.UtcNow;
        project.IsDeleted = request.IsDeleted;
        project.IsActive = request.IsActive;

        await _repository.UpdateAsync(project);

        return OperationResult<ProjectResponseDTO>.Success(MapEntityToDTO(project), "Project updated successfully.");
    }

    private static ProjectResponseDTO MapEntityToDTO(Project project)
    {
        return new ProjectResponseDTO
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            IsArchived = project.IsArchived,
            CreatedAt = project.CreatedAt,
            IsActive = project.IsActive,
            IsDeleted = project.IsDeleted,
            UpdatedAt = project.UpdatedAt,
        };
    }
}
