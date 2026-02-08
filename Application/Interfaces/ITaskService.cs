using Application.Common.Responses;
using Application.DTOs.Task;

namespace Application.Interfaces;

public interface ITaskService
{
    Task<OperationResult<TaskResponseDTO>> AddAsync(TaskRequestDTO request);
    Task<OperationResult<TaskResponseDTO>> UpdateAsync(TaskRequestUpdateDTO request);
    Task<OperationResult<List<TaskResponseDTO>>> GetAllAsync();
    Task<OperationResult<TaskResponseDTO>> GetByIdAsync(Guid id);
    Task<OperationResult<bool>> DeleteAsync(Guid id);
    Task<OperationResult<bool>> FindInProgressTasksAsync();
    Task<OperationResult<List<TaskResponseDTO>>> GetByProjectIdAsync(Guid id);
}
