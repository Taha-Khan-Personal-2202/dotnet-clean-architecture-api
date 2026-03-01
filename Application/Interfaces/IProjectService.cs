using Application.DTOs.Project;

namespace Application.Interfaces;

public interface IProjectService
{
    Task<OperationResult<ProjectResponseDTO>> CreateAsync(ProjectRequestDTO request);
    Task<OperationResult<ProjectResponseDTO>> GetByIdAsync(Guid id);
    Task<OperationResult<IEnumerable<ProjectResponseDTO>>> GetAllAsync();
    Task<OperationResult<ProjectResponseDTO>> UpdateAsync(ProjectRequestUpdateDTO request);
    Task<OperationResult<bool>> DeleteAsync(Guid id);           // no content → just success/fail
    Task<bool> ExistsByNameAsync(string name);
}