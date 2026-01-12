using Application.DTOs.Project;
using Application.DTOs.Task;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases.Tasks;

public class TaskUseCase(ITaskRepository repository,
    IProjectRepository projectRepository) : ITaskService
{
    public ITaskRepository _repository { get; set; } = repository;
    public IProjectRepository _projectRepository { get; set; } = projectRepository;

    public async Task<TaskResponseDTO> AddAsync(TaskRequestDTO request)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null)
            throw new KeyNotFoundException($"Project with ID {request.ProjectId} not found."); ;

        if (project.IsArchived)
            throw new Exception("Can not assing a task to archived project.");

        var task = new ProjectTask(
                    request.Title,
                    request.Description,
                    request.ProjectId,
                    request.DueDate);

        await _repository.AddAsync(task);

        return MapEntityToDTO(task);
    }

    public async Task DeleteAsync(Guid id)
    {
        var task = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Task with ID {id} not found.");

        await _repository.DeleteAsync(task);
    }

    public async Task<bool> FindInProgressTasksAsync()
    {
        return await _repository.FindCompletedTasksAsync();
    }

    public async Task<List<TaskResponseDTO>> GetAllAsync()
    {
        var tasks = await _repository.GetAllAsync();
        List<TaskResponseDTO> responseDTOs = new List<TaskResponseDTO>();

        tasks.ForEach(f =>
        {
            responseDTOs.Add(MapEntityToDTO(f));
        });

        return responseDTOs;
    }

    public async Task<TaskResponseDTO?> GetByIdAsync(Guid id)
    {
        var task = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Task with ID {id} not found.");
        return MapEntityToDTO(task);
    }

    public async Task<List<TaskResponseDTO>> GetByProjectIdAsync(Guid id)
    {
        if (await _projectRepository.GetByIdAsync(id) == null)
            throw new KeyNotFoundException($"Project with {id} not found.");

        List<ProjectTask> tasks = await _repository.GetByProjectIdAsync(id);

        List<TaskResponseDTO> responseDTOs = new();
        tasks.ForEach(f =>
        {
            responseDTOs.Add(MapEntityToDTO(f));
        });
        return responseDTOs;
    }

    public async Task UpdateAsync(TaskRequestUpdateDTO request)
    {
        var task = await _repository.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException($"Task with ID {request.Id} not found.");
        if (task.Status < request.Status) throw new Exception("Task status can not go backword.");

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.AssignedUserId = request.AssignedUserId;
        task.DueDate = request.DueDate;
        task.IsActive = request.IsActive;
        task.IsDeleted = request.IsDeleted;
        task.UpdatedAt = request.UpdatedAt;
        await _repository.UpdateAsync(task);
    }

    TaskResponseDTO MapEntityToDTO(ProjectTask task)
    {
        return new TaskResponseDTO()
        {
            Id = task.Id,
            Description = task.Description,
            AssignedUserId = task.AssignedUserId,
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
