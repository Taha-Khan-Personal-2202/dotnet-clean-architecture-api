using Application.DTOs.Tasks;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.UseCases.Tasks;

public class TaskUseCase(ITaskRepository repository) : ITaskService
{
    public ITaskRepository _repository { get; set; } = repository;

    public async Task<TaskResponseDTO> AddAsync(TaskRequestDTO request)
    {
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

    public async Task UpdateAsync(TaskRequestUpdateDTO request)
    {
        var task = await _repository.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException($"Task with ID {request.Id} not found.");
        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.AssignedUserId = request.AssignedUserId;
        task.DueDate = request.DueDate;
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
        };
    }
}
