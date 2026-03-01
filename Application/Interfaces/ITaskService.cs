using Application.DTOs.Task;

namespace Application.Interfaces;

public interface ITaskService
{
    Task<OperationResult<TaskResponseDTO>> AddAsync(TaskRequestDTO request);
    Task<OperationResult<TaskResponseDTO>> UpdateAsync(TaskRequestUpdateDTO request);
    Task<OperationResult<IEnumerable<TaskResponseDTO>>> GetAllAsync();
    Task<OperationResult<TaskResponseDTO>> GetByIdAsync(Guid id);
    Task<OperationResult<bool>> DeleteAsync(Guid id);
    Task<OperationResult<IEnumerable<TaskResponseDTO>>> GetByProjectIdAsync(Guid projectId);

    // Helper method used in ProjectService
    Task<bool> HasInProgressTasksForProjectAsync(Guid projectId);
}