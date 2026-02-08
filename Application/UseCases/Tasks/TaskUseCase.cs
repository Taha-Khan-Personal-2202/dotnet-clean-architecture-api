using Application.Common.Responses;
using Application.DTOs.Task;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.UseCases.Tasks;

public class TaskUseCase(ITaskRepository repository, IProjectRepository projectRepository) : ITaskService
{
    private readonly ITaskRepository _repository = repository;
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<OperationResult<TaskResponseDTO>> AddAsync(TaskRequestDTO request)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project is null)
            return OperationResult<TaskResponseDTO>.Fail($"Project with ID {request.ProjectId} not found.", 404);

        if (project.IsArchived)
            return OperationResult<TaskResponseDTO>.Fail("Cannot assign a task to an archived project.", 409);

        var task = new ProjectTask(request.Title, request.Description, request.ProjectId, request.DueDate);
        await _repository.AddAsync(task);

        return OperationResult<TaskResponseDTO>.Success(MapEntityToDTO(task), "Task created successfully.", 201);
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task is null)
            return OperationResult<bool>.Fail($"Task with ID {id} not found.", 404);

        await _repository.DeleteAsync(task);
        return OperationResult<bool>.Success(true, "Task deleted successfully.");
    }

    public async Task<OperationResult<bool>> FindInProgressTasksAsync()
    {
        var hasOpenTasks = await _repository.FindCompletedTasksAsync();
        return OperationResult<bool>.Success(hasOpenTasks, "Task progress check completed.");
    }

    public async Task<OperationResult<List<TaskResponseDTO>>> GetAllAsync()
    {
        var tasks = await _repository.GetAllAsync();
        var responseDTOs = tasks.Select(MapEntityToDTO).ToList();
        return OperationResult<List<TaskResponseDTO>>.Success(responseDTOs, "Tasks fetched successfully.");
    }

    public async Task<OperationResult<TaskResponseDTO>> GetByIdAsync(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task is null)
            return OperationResult<TaskResponseDTO>.Fail($"Task with ID {id} not found.", 404);

        return OperationResult<TaskResponseDTO>.Success(MapEntityToDTO(task), "Task fetched successfully.");
    }

    public async Task<OperationResult<List<TaskResponseDTO>>> GetByProjectIdAsync(Guid id)
    {
        if (await _projectRepository.GetByIdAsync(id) is null)
            return OperationResult<List<TaskResponseDTO>>.Fail($"Project with ID {id} not found.", 404);

        var tasks = await _repository.GetByProjectIdAsync(id);
        var responseDTOs = tasks.Select(MapEntityToDTO).ToList();

        return responseDTOs.Count == 0
            ? OperationResult<List<TaskResponseDTO>>.Fail("No tasks found for this project.", 404)
            : OperationResult<List<TaskResponseDTO>>.Success(responseDTOs, "Tasks fetched successfully.");
    }

    public async Task<OperationResult<TaskResponseDTO>> UpdateAsync(TaskRequestUpdateDTO request)
    {
        var task = await _repository.GetByIdAsync(request.Id);
        if (task is null)
            return OperationResult<TaskResponseDTO>.Fail($"Task with ID {request.Id} not found.", 404);

        if (task.Status > request.Status)
            return OperationResult<TaskResponseDTO>.Fail("Task status cannot move backward.", 409);

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.DueDate = request.DueDate;
        task.IsActive = request.IsActive;
        task.IsDeleted = request.IsDeleted;
        task.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(task);

        return OperationResult<TaskResponseDTO>.Success(MapEntityToDTO(task), "Task updated successfully.");
    }

    private static TaskResponseDTO MapEntityToDTO(ProjectTask task)
    {
        return new TaskResponseDTO
        {
            Id = task.Id,
            Description = task.Description,
            CreatedAt = task.CreatedAt,
            DueDate = task.DueDate,
            ProjectId = task.ProjectId,
            Status = task.Status,
            Title = task.Title,
            UpdatedAt = task.UpdatedAt,
            IsDeleted = task.IsDeleted,
            IsActive = task.IsActive,
        };
    }
}
