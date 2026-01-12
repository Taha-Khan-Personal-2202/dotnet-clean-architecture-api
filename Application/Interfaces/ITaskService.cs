using Application.DTOs.Task;

namespace Application.Interfaces;

public interface ITaskService
{
    Task<TaskResponseDTO> AddAsync(TaskRequestDTO request);
    Task UpdateAsync(TaskRequestUpdateDTO request);
    Task<List<TaskResponseDTO>> GetAllAsync();
    Task<TaskResponseDTO?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<bool> FindInProgressTasksAsync();
}
