using Application.DTOs.Task;
using Application.Interfaces;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;

namespace Application.UseCases.Tasks;

public sealed class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IProjectRepository _projectRepository;
    private readonly IValidator<TaskRequestDTO> _createValidator;
    private readonly IValidator<TaskRequestUpdateDTO> _updateValidator;

    public TaskService(
        ITaskRepository repository,
        IProjectRepository projectRepository,
        IValidator<TaskRequestDTO> createValidator,
        IValidator<TaskRequestUpdateDTO> updateValidator)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
    }

    public async Task<OperationResult<TaskResponseDTO>> AddAsync(TaskRequestDTO request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errors = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage));
            return OperationResult<TaskResponseDTO>.Error(errors, 400);
        }

        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project is null)
            return OperationResult<TaskResponseDTO>.Error("Project not found.", 404);

        if (project.IsArchived)
            return OperationResult<TaskResponseDTO>.Error("Cannot assign task to an archived project.", 400);

        var task = new ProjectTask(
            title: request.Title.Trim(),
            description: request.Description?.Trim(),
            projectId: request.ProjectId,
            dueDate: request.DueDate);

        await _repository.AddAsync(task);
        
        return OperationResult<TaskResponseDTO>.Ok(
            MapToDto(task),
            "Task created successfully.",
            201);
    }

    public async Task<OperationResult<TaskResponseDTO>> GetByIdAsync(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task is null)
            return OperationResult<TaskResponseDTO>.Error("Task not found.", 404);

        return OperationResult<TaskResponseDTO>.Ok(MapToDto(task));
    }

    public async Task<OperationResult<IEnumerable<TaskResponseDTO>>> GetAllAsync()
    {
        var tasks = await _repository.GetAllAsync();
        var dtos = tasks.Select(MapToDto).ToList();

        return OperationResult<IEnumerable<TaskResponseDTO>>.Ok(
            dtos,
            $"Retrieved {dtos.Count} tasks.");
    }

    public async Task<OperationResult<TaskResponseDTO>> UpdateAsync(TaskRequestUpdateDTO request)
    {
        var validation = await _updateValidator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errors = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage));
            return OperationResult<TaskResponseDTO>.Error(errors, 400);
        }

        var task = await _repository.GetByIdAsync(request.Id);
        if (task is null)
            return OperationResult<TaskResponseDTO>.Error("Task not found.", 404);

        if (request.Status < task.Status)
            return OperationResult<TaskResponseDTO>.Error("Task status cannot be downgraded.", 400);

        task.Title = request.Title.Trim();
        task.Description = request.Description?.Trim();
        task.Status = request.Status;
        task.DueDate = request.DueDate;
        task.IsActive = request.IsActive;
        task.IsDeleted = request.IsDeleted;
        task.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(task);
        
        return OperationResult<TaskResponseDTO>.Ok(
            MapToDto(task),
            "Task updated successfully.");
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task is null)
            return OperationResult<bool>.Error("Task not found.", 404, false);

        await _repository.DeleteAsync(task);
        
        return OperationResult<bool>.Ok(true, "Task deleted successfully.");
    }

    public async Task<OperationResult<IEnumerable<TaskResponseDTO>>> GetByProjectIdAsync(Guid projectId)
    {
        var projectExists = await _projectRepository.GetByIdAsync(projectId);
        if (projectExists == null)
            return OperationResult<IEnumerable<TaskResponseDTO>>.Error("Project not found.", 404);

        var tasks = await _repository.GetByProjectIdAsync(projectId);
        var dtos = tasks.Select(MapToDto).ToList();

        return OperationResult<IEnumerable<TaskResponseDTO>>.Ok(
            dtos,
            $"Retrieved {dtos.Count} tasks for project {projectId}.");
    }

    public async Task<bool> HasInProgressTasksForProjectAsync(Guid projectId)
    {
        var tasks = await _repository.GetByProjectIdAsync(projectId);
        return tasks.Any(t => t.Status == Status.InProgress || t.Status == Status.Pending);
    }

    private static TaskResponseDTO MapToDto(ProjectTask task) => new()
    {
        Id = task.Id,
        Title = task.Title,
        Description = task.Description,
        Status = task.Status,
        ProjectId = task.ProjectId,
        DueDate = task.DueDate,
        CreatedAt = task.CreatedAt,
        UpdatedAt = task.UpdatedAt,
        IsActive = task.IsActive,
        IsDeleted = task.IsDeleted
    };
}